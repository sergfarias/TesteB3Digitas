using MongoDB.Bson;
using System.Threading.Tasks;
using TesteDigitas.Domain.Models;
using System.Collections.Generic;
using TesteDigitas.Application.ViewModel;
using TesteDigitas.Application.Interfaces.Enum;
namespace TesteDigitas.Application.Services.BitStamp
{
    public interface IOrderBookService
    {
        Task<Response> ReturnData(string Moeda);
        Task<ObjectId> InsertData(ReturnOrderBookViewModel listOrders);
        Task<List<FindOrderBookViewModel>> Calculate(Operacao Operacao);
    }
}
