using MongoDB.Driver;
using System.Threading.Tasks;
using TesteDigitas.Application.ViewModel;
namespace TesteDigitas.Application.Services.MongoDb
{
    public interface IMongoDbService
    {
        Task<IMongoCollection<ReturnOrderBookViewModel>> ConnectMongoDbOrdersAsync();
        IMongoCollection<ReturnOrderBookViewModel> ConnectMongoDbOrders();
        Task<IMongoCollection<ReturnSimulationViewModel>> ConnectMongoDbCalculaMemoryAsync();
    }
}
