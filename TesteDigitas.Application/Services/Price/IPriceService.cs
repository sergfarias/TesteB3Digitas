using System.Threading.Tasks;
using System.Collections.Generic;
using TesteDigitas.Application.ViewModel;
using TesteDigitas.Application.Interfaces.Enum;
namespace TesteDigitas.Application.Services.Price
{
    public interface IPriceService
    {
        Task<ReturnSimulationViewModel>  BestPrice(Operacao Operacao, string Instrumento, int Quantidade);
        Task<List<ReturnSimulationViewModel>> SearchSimulations();
    }
}
