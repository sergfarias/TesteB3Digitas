using System;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using System.Threading;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using TesteDigitas.Domain.Models;
using System.Collections.Generic;
using TesteDigitas.Application.ViewModel;
using TesteDigitas.Application.Interfaces.Enum;
using TesteDigitas.Application.Services.MongoDb;
namespace TesteDigitas.Application.Services.BitStamp
{
    public class OrderBookService : IOrderBookService 
    {
        private IMongoDbService _mongoDbService;
        public OrderBookService(IMongoDbService mongoDbService) 
        {
            _mongoDbService = mongoDbService;
        }

        #region Buscar e gravar Mongdb

          public async Task<Response> ReturnData(string Moeda)
          {
            try
            {
                string jsonString = await RetornSocket(Moeda);
                if (string.IsNullOrEmpty(jsonString))
                {
                    return new Response
                    {
                         Message = "Nenhum dado retornado!",
                         StatusCode = "400"
                    };
                }

                var Id = await InsertData(JsonSerializer.Deserialize<ReturnOrderBookViewModel>(jsonString));

                var response = new Response();
                if (!Id.Equals(null))
                {
                    response.Message = "Gravado com sucesso!";
                    response.StatusCode = "200";
                    Console.Write("ID " + Id + "  gravado com sucesso!"+"\n");
                }
                else
                {
                    response.Message = "Falha na inclusão!";
                    response.StatusCode = "400";
                }
                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    Message = ex.Message,
                    StatusCode = "400"
                };
                return response;
            }
        }

        public async Task<string> ReturnSocketSBitstamp(string Moeda)
        {
            try
            {
                var buffer = new byte[1024 * 4];
                string query = "{\"event\": \"bts:subscribe\",\"data\":{\"channel\": \"order_book_" + Moeda + "\"}" + "}";

                using var ws = new ClientWebSocket();
                await ws.ConnectAsync(new Uri("wss://ws.bitstamp.net"), CancellationToken.None);
                var encoded = Encoding.UTF8.GetBytes(query);
                var buffer2 = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                await ws.SendAsync(buffer2, WebSocketMessageType.Text, true, CancellationToken.None);

                string jsonString = "";
                int i = 0;
                while (i <= 2)
                {
                    i++;
                    var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                    else
                    {
                        if (i > 1)
                        {
                            jsonString += Encoding.ASCII.GetString(buffer, 0, result.Count);
                        }
                    }
                }
                return jsonString;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<ObjectId> InsertData(ReturnOrderBookViewModel listOrders)
        {
            var collection = await _mongoDbService.ConnectMongoDbOrdersAsync();
            await collection.InsertOneAsync(listOrders);
            return listOrders._id;
        }
        #endregion


        #region Exercicio 2
       
        public async Task<List<FindOrderBookViewModel>> Calculate(Operacao Operacao)
        {
            try 
            {
                var lstData = await DataBase();
                if (lstData != null)
                {

                    //Maior preço
                    var lstMaxPrice = MaxPrice(lstData, Operacao);
                    var lstResponse = new List<FindOrderBookViewModel>();
                    lstResponse.AddRange(lstMaxPrice);

                    //Menor preço
                    var lstMinusPrice = MinorPrice(lstData, Operacao);
                    foreach (var item in lstResponse)
                    {
                        foreach (var item2 in lstMinusPrice)
                        {
                            if (item.Channel == item2.Channel)
                                item.MinorPreci = item2.MinorPreci;
                        }
                    }

                    //média preço
                    var lstMediaPrice = AvgPrice(lstData, Operacao);
                    foreach (var item in lstResponse)
                    {
                        foreach (var item2 in lstMediaPrice)
                        {
                            if (item.Channel == item2.Channel)
                                item.AveragePreci = item2.AveragePreci;
                        }
                    }

                    ////média preço nos ultimos 5 segundos
                    var lstMediaUlt5Segundos = await AvgPriceLastFiveSeconds(Operacao);
                    if (lstMediaUlt5Segundos != null)
                    {
                        foreach (var item in lstResponse)
                        {
                            foreach (var item2 in lstMediaUlt5Segundos)
                            {
                                if (item.Channel == item2.Channel)
                                    item.AveragePreciFiveSeconds = item2.AveragePreciFiveSeconds;
                            }
                        }
                    }

                    //o Média de quantidade acumulada de cada ativo 
                    var lstMediaAcumulada = await AvgQuantityAccumulated("order_book_btcusd", Operacao);
                    var recebe = await AvgQuantityAccumulated("order_book_ethusd", Operacao);
                    if (recebe != null)
                        lstMediaAcumulada.AddRange(recebe);

                    foreach (var item in lstResponse)
                    {
                        if (lstMediaAcumulada != null)
                        {
                            foreach (var item2 in lstMediaAcumulada)
                            {
                                if (item.Channel == item2.Channel)
                                    item.AverageQuantityAccumulate = item2.AverageQuantityAccumulate;
                            }
                        }
                    }

                    foreach (var item in lstResponse)
                        item.Operation = (Operacao.Compra == Operacao ? "Compra" : "Venda");

                    return lstResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Busca base no momento, pego o último inserido
        public async Task<List<ReturnOrderBookViewModel>> DataBase()
        {
            try
            {
                var collection = await _mongoDbService.ConnectMongoDbOrdersAsync();
                return collection.Find(x => true && (x.channel.Equals("order_book_btcusd") || x.channel.Equals("order_book_ethusd"))).SortByDescending(d => d._id).Limit(2).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Maior preço cada ativo no momento
        public List<FindOrderBookViewModel> MaxPrice(List<ReturnOrderBookViewModel> lstData, Operacao Operacao)
        {
            try {
                var lista = new List<FindOrderBookViewModel>();

                for (var i=0;  i< lstData.Count; i++)
                {
                    var item = new FindOrderBookViewModel{Channel = lstData[i].channel};

                    double maior = 0;
                    if (Operacao.Venda == Operacao)
                    {
                        var n = lstData[i].data.bids.Count;
                        maior = double.Parse(lstData[i].data.bids[0][1].Replace(".", ","));
                        for (var j = 0; j < n; j++)
                        {
                            if (double.Parse(lstData[i].data.bids[j][1].Replace(".", ",")) > maior)
                                maior = double.Parse(lstData[i].data.bids[j][1].Replace(".", ","));
                        }
                    }
                    else
                    {
                        var n = lstData[i].data.asks.Count;
                        maior = double.Parse(lstData[i].data.asks[0][1].Replace(".", ","));
                        for (var j = 0; j < n; j++)
                        {
                            if (double.Parse(lstData[i].data.asks[j][1].Replace(".", ",")) > maior)
                                maior = double.Parse(lstData[i].data.asks[j][1].Replace(".", ","));
                        }
                    }

                    item.BigPreci = maior;

                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception)
            {
                  return null;
            }
        }

        //Menor preço cada ativo no momento
        public List<FindOrderBookViewModel> MinorPrice(List<ReturnOrderBookViewModel> lstData, Operacao Operacao)
        {
            try
            {
                var lista = new List<FindOrderBookViewModel>();

                for (var i = 0; i < lstData.Count; i++)
                {
                    var item = new FindOrderBookViewModel
                    {
                        Channel = lstData[i].channel
                    };

                    double menor = 0;
                    if (Operacao.Venda == Operacao)
                    {
                        var n = lstData[i].data.bids.Count;
                        menor = double.Parse(lstData[i].data.bids[0][1].Replace(".", ","));
                        for (var j = 0; j < n; j++)
                        {
                            if (double.Parse(lstData[i].data.bids[j][1].Replace(".", ",")) < menor)
                                menor = double.Parse(lstData[i].data.bids[j][1].Replace(".", ","));
                        }
                    }
                    else
                    {
                        var n = lstData[i].data.asks.Count;
                        menor = double.Parse(lstData[i].data.asks[0][1].Replace(".", ","));
                        for (var j = 0; j < n; j++)
                        {
                            if (double.Parse(lstData[i].data.asks[j][1].Replace(".", ",")) < menor)
                                menor = double.Parse(lstData[i].data.asks[j][1].Replace(".", ","));
                        }
                    }

                    item.MinorPreci = menor;

                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Média preço cada ativo no momento
        public List<FindOrderBookViewModel> AvgPrice(List<ReturnOrderBookViewModel> lstData, Operacao Operacao)
        {
            try
            {
                var lista = new List<FindOrderBookViewModel>();

                for (var i = 0; i < lstData.Count; i++)
                {
                    var item = new FindOrderBookViewModel
                    {
                        Channel = lstData[i].channel
                    };

                    double Total = 0;
                    int n = 0;
                    if (Operacao.Venda == Operacao)
                    {
                        n = lstData[i].data.bids.Count;
                        for (var j = 0; j < n; j++)
                        {
                            Total += double.Parse(lstData[i].data.bids[j][1].Replace(".", ","));
                        }
                    }
                    else
                    {
                        n = lstData[i].data.asks.Count;
                        for (var j = 0; j < n; j++)
                        {
                            Total += double.Parse(lstData[i].data.asks[j][1].Replace(".", ","));
                        }
                    }

                    item.AveragePreci = Total/n;

                    lista.Add(item);
                }

                return lista;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<FindOrderBookViewModel>> AvgPriceLastFiveSeconds(Operacao Operacao)
        {
            try
            {
                var collection =await _mongoDbService.ConnectMongoDbOrdersAsync();
                if (collection != null)
                {
                    var data = collection.Find(x => true && (x.channel.Equals("order_book_btcusd")
                                            || x.channel.Equals("order_book_ethusd"))
                                            ).SortByDescending(d => d._id).Limit(100).ToList();

                    data = data.Where(c => (
                                              (TimeZoneInfo.ConvertTimeToUtc(DateTime.Now) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds
                                            - Convert.ToDouble(c.data.timestamp)
                                            ) <= 5).ToList();

                    var lista = new List<FindOrderBookViewModel>();

                    //int qtd=0;
                    double Total = 0;
                    int n = 0;
                    string Channel = "";
                    foreach (var item in data.Select(c => new { c.channel }).Distinct())
                    {
                        //order_book_btcusd || order_book_ethusd
                        for (var i = 0; i < data.Where(c => c.channel == item.channel).Count(); i++)
                        {
                            Channel = item.channel;
                            if (Operacao.Venda == Operacao)
                            {
                                n = data[i].data.bids.Count;
                                for (var j = 0; j < n; j++)
                                    Total += double.Parse(data[i].data.bids[j][1].Replace(".", ","));
                            }
                            else
                            {
                                n = data[i].data.asks.Count;
                                for (var j = 0; j < n; j++)
                                    Total += double.Parse(data[i].data.asks[j][1].Replace(".", ","));
                            }
                        }

                        lista.Add(new FindOrderBookViewModel
                        {
                            Channel = Channel,
                            AveragePreciFiveSeconds = Total / n
                        });

                        Channel = "";
                        Total = 0;
                        n = 0;
                    }
                    return lista;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<FindOrderBookViewModel>> AvgQuantityAccumulated(string Channel, Operacao Operacao)
        {
            try
            {
                var collection = await _mongoDbService.ConnectMongoDbOrdersAsync();
                var dados = collection.Find(x => true && (x.channel.Equals(Channel))).ToList();
        
                var lista = new List<FindOrderBookViewModel>();
                int n = 0;
                double Total = 0;
                for (var i = 0; i < dados.Count; i++)
                {
                    if (Operacao.Venda == Operacao)
                    {
                        n = dados[i].data.bids.Count;
                        for (var j = 0; j < n; j++)
                            Total += double.Parse(dados[i].data.bids[j][1].Replace(".", ","));
                    }
                    else
                    {
                        n = dados[i].data.asks.Count;
                        for (var j = 0; j < n; j++)
                            Total += double.Parse(dados[i].data.asks[j][1].Replace(".", ","));
                    }
                }

                lista.Add(new FindOrderBookViewModel{ Channel = Channel, 
                                                      AverageQuantityAccumulate = Total / n});
                return lista;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
}
