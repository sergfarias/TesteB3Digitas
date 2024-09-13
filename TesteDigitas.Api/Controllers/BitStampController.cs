using Microsoft.AspNetCore.Mvc;
using TesteDigitas.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using TesteDigitas.Application.Services.Price;
using TesteDigitas.Application.Services.BitStamp;
using TesteDigitas.Application.Interfaces.Enum;

namespace TesteDigitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BitStampController : ControllerBase
    {
        private readonly IOrderBookService _orderBookService;
        private readonly IPriceService _priceService;
        public BitStampController(IOrderBookService orderBookService, IPriceService priceService)
        {
            this._orderBookService = orderBookService;
            this._priceService = priceService;
        }

        // POST api/<IntegrationController>/3
        [HttpPost]
        [Route("OrderBook")]
        public async Task<ActionResult> OrderBook()
        {
            var listResponse = new List<Response>
            {
                await _orderBookService.ReturnData("btcusd"),
                await _orderBookService.ReturnData("ethusd")
            };
            return Ok(listResponse);
        }

        [HttpPost]
        [Route("Calculate")]
        public async Task<ActionResult> Calculate()
        {
            var listResponse = await _orderBookService.Calculate(Operacao.Compra);
            listResponse.AddRange(_orderBookService.Calculate(Operacao.Venda).Result);
            return Ok(listResponse);
        }

        /// <summary>
        /// Expor uma API de simulação de melhor preço:
        /// </summary>
        /// <remarks>
        /// Expor uma API de simulação de melhor preço:
        /// </remarks>
        /// <param name="operacao">Compra(1) ou Venda(0).</param>
        /// <param name="produto">"order_book_btcusd" ou "order_book_ethusd".</param>
        /// <param name="quantidade">Quatidade.</param>
        [HttpPost]
        [Route("BestPrice")]
        public async Task<ActionResult> BestPrice(Operacao operacao, string produto, int quantidade)
        {
            var listResponse = await _priceService.BestPrice(operacao, produto, quantidade);
            if (listResponse != null)
                return Ok(listResponse);
            else
                return BadRequest("Nenhum dado encontrado!");
        }

        [HttpGet]
        [Route("SearchSimulations")]
        public async Task<ActionResult> SearchSimulations()
        {
            var listResponse = await _priceService.SearchSimulations();
            return Ok(listResponse);
        }
    }
}
