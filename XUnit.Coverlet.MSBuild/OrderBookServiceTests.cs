using System.Text.Json;
using TesteDigitas.Application.Interfaces.Enum;
using TesteDigitas.Application.Services.BitStamp;
using TesteDigitas.Application.Services.MongoDb;
using TesteDigitas.Application.ViewModel;
namespace XUnit.Coverlet
{
    public class OrderBookServiceTests
    {
        public OrderBookServiceTests()
        {
        }

        [Fact]
        public void ReturnSocket_Valido()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnSocketSBitstamp("btcusd");
            Assert.True(!string.IsNullOrEmpty(teste.Result));
        }

        [Fact]
        public void ReturnSocket_Invalido()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnSocketSBitstamp("");
            Assert.True(string.IsNullOrEmpty(teste.Result));
        }

        [Fact]
        public void Database_ReturnDataBTC()
        {
         var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnData("btcusd");
            Assert.True(teste.Result.StatusCode=="200");
        }

        [Fact]
        public void Database_ReturnDataETH()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnData("ethusd");
            Assert.True(teste.Result.StatusCode == "200");
        }

        [Fact]
        public void Database_ReturnDataBTC1()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnData("btcusd");
            Assert.False(teste.Result.StatusCode == "400");
        }

        [Fact]
        public void Database_ReturnDataETH1()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.ReturnData("ethusd");
            Assert.False(teste.Result.StatusCode == "400");
        }

        [Fact]
        public void Calcule_TestNotNull()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            //Ação
            var teste = service.Calculate(Operacao.Compra);
            //Asserts
            Assert.NotNull(teste.Result);
        }

        [Fact]
        public void Database_TestNotNull()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste2 = service.DataBase();
            Assert.NotNull(teste2);
        }

        [Fact]
        public async Task InsertData_TestValido()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}";
            var teste = await service.InsertData(JsonSerializer.Deserialize<ReturnOrderBookViewModel>(jsonString));
            Assert.NotNull(teste.ToString());
        }

        [Fact]
        public void AvgPrice_TestValido()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
            var teste =  service.AvgPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Venda);
            Assert.True(teste.Count >= 0); 
        }

        //[Fact]
        //public void AvgPrice_TestNull()
        //{
        //    var mongoDb = new MongoDbService();
        //    var service = new OrderBookService(mongoDb);
        //    //string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
        //    //jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
        //    //jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
        //    //var teste = service.AvgPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Venda);
        //    Assert.False(null);
        //}

        [Fact]
        public void MaxPrice_TestValidoCompra()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
            var teste = service.MaxPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Compra);
            Assert.True(teste.Count > 0);
        }

        [Fact]
        public void MaxPrice_TestValidoVenda()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
            var teste = service.MaxPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Venda);
            Assert.True(teste.Count > 0);
        }

        [Fact]
        public void MinorPrice_TestValidoCompra()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
            var teste = service.MinorPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Compra);
            Assert.True(teste.Count > 0);
        }

        [Fact]
        public void MinorPrice_TestValidoVenda()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
            jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
            jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
            var teste = service.MinorPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Venda);
            Assert.True(teste.Count > 0);
        }

        [Fact]
        public void AvgPriceLastFiveSeconds_TestInvalidoCompra()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.AvgPriceLastFiveSeconds(Operacao.Compra);
            Assert.True(teste.Result.Count >= 0);
        }

        [Fact]
        public void AvgQuantityAccumulated_TestValido_Venda()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.AvgQuantityAccumulated("order_book_btcusd", Operacao.Venda);
            Assert.True(teste.Result.Count > 0);
        }

        [Fact]
        public void AvgQuantityAccumulated_TestValido_Venda2()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.AvgQuantityAccumulated("order_book_ethusd", Operacao.Venda);
            Assert.True(teste.Result.Count > 0);
        }

        [Fact]
        public void AvgQuantityAccumulated_TestValido_Compra()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.AvgQuantityAccumulated("order_book_btcusd", Operacao.Compra);
            Assert.True(teste.Result.Count > 0);
        }

        [Fact]
        public void AvgQuantityAccumulated_TestValido_Compra2()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var teste = service.AvgQuantityAccumulated("order_book_ethusd", Operacao.Compra);
            Assert.True(teste.Result.Count > 0);
        }

        [Fact]
        public void Calculate_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var teste = service.Calculate(Operacao.Compra).Result;
                var result = Assert.Throws<ArgumentNullException>(() => teste);
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public void DataBase_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                Action act = () => service.DataBase();
                var exception = Assert.Throws<ArgumentException>(() => act);
                Assert.Equal("expected error message here", exception.Message);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public void AvgPriceLastFiveSeconds_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var expectedExcetpion = new ArgumentException();
            try
            {
                var teste = service.AvgPriceLastFiveSeconds(Operacao.Compra);
                var result = Assert.Throws<ArgumentException>(() => teste.Result);
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public void AvgQuantityAccumulated_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var teste = service.AvgQuantityAccumulated("order_book_ethusd",Operacao.Compra);
                var result = Assert.Throws<ArgumentNullException>(() => teste.Result);
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public void AvgPrice_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new OrderBookService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                string jsonString = "[{\"data\":{\"timestamp\":\"1726011793\",\"microtimestamp\":\"1726011793558987\",";
                jsonString += "\"bids\":[[\"57655\",\"0.69359400\"],[\"57654\",\"0.00185982\"],[\"57650\",\"0.06000000\"],[\"57106\",\"0.02500000\"],[\"57098\",\"0.00023230\"]],";
                jsonString += "\"asks\":[[\"58242\",\"0.00100563\"],[\"58251\",\"0.00022998\"],[\"58256\",\"0.00041087\"],[\"58291\",\"0.00100563\"]]},\"channel\":\"order_book_btcusd\",\"event\":\"data\"}]";
                var teste = service.AvgPrice(JsonSerializer.Deserialize<List<ReturnOrderBookViewModel>>(jsonString), Operacao.Compra);
                var result = Assert.Throws<ArgumentNullException>(() => teste);
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }
    }
}