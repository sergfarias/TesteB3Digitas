using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;
using TesteDigitas.Application.Settings;
using TesteDigitas.Application.ViewModel;
namespace TesteDigitas.Application.Services.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        private readonly string _connectionString;
        public MongoDbService()
        {
            _connectionString = "mongodb+srv://sergfarias:yh7TVQzaseaHTybZ@cluster0.j57sy.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        }

        public async Task<IMongoCollection<ReturnSimulationViewModel>> ConnectMongoDbCalculaMemoryAsync()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("sample_order");
            var collection = database.GetCollection<ReturnSimulationViewModel>("calculate");
            return collection;
        }

        public async Task<IMongoCollection<ReturnOrderBookViewModel>> ConnectMongoDbOrdersAsync()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("sample_order");
            var collection = database.GetCollection<ReturnOrderBookViewModel>("orders");
            return collection;
        }

        public IMongoCollection<ReturnOrderBookViewModel> ConnectMongoDbOrders()
        {   
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase("sample_order");
            var collection = database.GetCollection<ReturnOrderBookViewModel>("orders");
            return collection;
        }

    }
}
