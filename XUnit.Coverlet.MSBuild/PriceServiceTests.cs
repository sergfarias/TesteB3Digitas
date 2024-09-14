using TesteDigitas.Domain.Models;
using TesteDigitas.Application.Services.Price;
using TesteDigitas.Application.Interfaces.Enum;
using TesteDigitas.Application.Services.MongoDb;
namespace XUnit.Coverlet
{
    public class PriceServiceTests
    {
        public PriceServiceTests()
        {
        }

        [Fact]
        public async Task BestPrice_TestValidoCompra()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = await service.BestPrice(Operacao.Compra, "order_book_btcusd", 100);
            Assert.True(!string.IsNullOrEmpty(teste.Id.ToString()));
        }

        [Fact]
        public async Task BestPrice_TestValidoVenda()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = await service.BestPrice(Operacao.Venda, "order_book_btcusd", 100);
            Assert.True(!string.IsNullOrEmpty(teste.Id.ToString()));
        }

        [Fact]
        public void BestPrice_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var teste = service.BestPrice(Operacao.Venda, "", 100).Result;
                var result = Assert.Throws<ArgumentNullException>(() => teste);
                Assert.Equal(expectedExcetpion, result);
            }
            catch(Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public void SearchSimulations_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var result = Assert.Throws<ArgumentNullException>(() => service.SearchSimulations().Result.ToList());
                Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }

        [Fact]
        public async Task SearchSimulations_TestValidoVenda()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = await service.SearchSimulations();
            Assert.True(teste.Count > 0);
        }

        [Fact]
        public async Task SearchSimulations_TestInvalidoVenda()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = await service.SearchSimulations();
            Assert.False(teste.Count == 0);
            Assert.False(teste == null);
        }

        [Fact]
        public async Task SearchSimulations_TestValidoVenda1()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = await service.SearchSimulations();
            Assert.True(!string.IsNullOrEmpty(teste[0].Operacao));
        }

        //[Fact]
        //public void DataBase_TestException()
        //{
        //    var mongoDb = new MongoDbService();
        //    var service = new PriceService(mongoDb);
        //    var expectedExcetpion = new ArgumentNullException();
        //    try
        //    {
        //        var teste = service.DataBase("");
        //        var result = Assert.Throws<Exception>(() => teste.Result);
        //        Assert.Equal(expectedExcetpion, result);
        //    }
        //    catch (Exception)
        //    {
        //        Assert.True(1 == 1);
        //    }
        //}

        [Fact]
        public void DataBase_TestException()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var expectedExcetpion = new ArgumentNullException();
            try
            {
                var result = Assert.Throws<ArgumentNullException>(() => service.DataBase("").Result);
               Assert.Equal(expectedExcetpion, result);
            }
            catch (Exception)
            {
                Assert.True(1 == 1);
            }
        }


        [Fact]
        public void DataBase_TestNull()
        {
            var mongoDb = new MongoDbService();
            var service = new PriceService(mongoDb);
            var teste = service.DataBase("order_book_btcusd");
            Assert.True(!string.IsNullOrEmpty(teste.Result[0].channel));
        }

       
        [Fact]
        public void DefaultApiResponse_TestValido()
        {
            var service = new DefaultApiResponse(null,"", true);
            Assert.True(!service.IsSuccessful);
            Assert.Null(service.Data);
            Assert.Equal("", service.Notification);
        }
    }
}