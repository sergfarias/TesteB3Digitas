using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TesteDigitas.Application.ViewModel;
using TesteDigitas.Application.Interfaces.Enum;
using TesteDigitas.Application.Services.MongoDb;
namespace TesteDigitas.Application.Services.Price
{
    public class PriceService : IPriceService 
    {
        private IMongoDbService _mongoDbService;
        public PriceService( IMongoDbService mongoDbService) 
        {
            _mongoDbService = mongoDbService;
        }
   
        public async Task<ReturnSimulationViewModel> BestPrice(Operacao Operacao, string Instrumento, int Quantidade)
        {
            try
            {
                var data = DataBase(Instrumento);

                double ValorTotal = 0;
                var ColecaoUsada = new List<string>();
              
                var Ordenado = OrderVetor(Operacao, data);
                int conta = 0;
                foreach (var item in Ordenado)
                {
                    if (ValorTotal < Quantidade)
                    {    
                        ValorTotal += (Quantidade * double.Parse(item[1].Replace(".", ",")));
                        conta++;
                        var linha = new { V = item[0], V1 = item[1] };
                        ColecaoUsada.Add(linha.ToString());
                    }
                    else
                        break;
                }
              
                var response = new ReturnSimulationViewModel
                {
                    Id = Guid.NewGuid(),
                    Quatidade = Quantidade,
                    Operacao = (Operacao.Compra == Operacao ? "Compra" : "Venda"),
                    Resultado = ValorTotal,
                    ColecaoUsada = ColecaoUsada,
                    DataCriacao = DateTime.Now.ToString()
                };

                var Id = await InsertData(response);

                return response;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Guid> InsertData(ReturnSimulationViewModel listSimulation)
        {
            var collection = await _mongoDbService.ConnectMongoDbCalculaMemoryAsync();
            await collection.InsertOneAsync(listSimulation);
            return listSimulation.Id;
        }

        //Busca base no momento, pego o último inserido
        public List<ReturnOrderBookViewModel> DataBase(string instrumento)
        {
            try
            {
                var collection = _mongoDbService.ConnectMongoDbOrders();
                return collection.Find(x => true && x.channel.Equals(instrumento)).SortByDescending(d => d._id).Limit(1).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<ReturnSimulationViewModel>> SearchSimulations()
        {
            try
            {
                var collection = await _mongoDbService.ConnectMongoDbCalculaMemoryAsync();
                return collection.Find(c=> true).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IOrderedEnumerable<string[]> OrderVetor(Operacao Operacao, List<ReturnOrderBookViewModel> data)
        {
            if (Operacao.Compra == Operacao)
                return data[0].data.asks.OrderBy(l => double.Parse(l[1].Replace(".", ",")));
            else
                return data[0].data.bids.OrderByDescending(l => double.Parse(l[1].Replace(".", ",")));
        }

    }
}
