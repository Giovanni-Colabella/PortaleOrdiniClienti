using API.Models.DTO;
using API.Models.Services.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrelloController : ControllerBase
    {

        private readonly ICarrelloService _carrelloService;
        public CarrelloController(ICarrelloService carrelloService)
        {
            _carrelloService = carrelloService;
        }

        [HttpGet]
        public async Task<List<ProdottoResponseDto>> GetArticoliFromCarrello(int idCliente)
        {
            return await _carrelloService.GetArticoliFromCarrelloAsync(idCliente);
        }

        [HttpPost]
        public async Task AggiungiAlCarrello(int idCliente, int prodottoId)
        {
            await _carrelloService.AggiungiAlCarrelloAsync(idCliente, prodottoId);
        }
    }
}
