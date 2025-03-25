using System.Security.Claims;
using API.Models.DTO;
using API.Models.Services.Application;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<List<ProdottoResponseDto>> GetArticoliFromCarrello()
        {
            var idCliente = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idCliente == null)
                throw new Exception("Utente non trovato");
            return await _carrelloService.GetArticoliFromCarrelloAsync(idCliente);
        }

        [Authorize]
        [HttpPost]
        public async Task AggiungiAlCarrello([FromBody] int prodottoId)
        {
            // Recupera l'id dell'utente loggato
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception("Utente non trovato");
            await _carrelloService.AggiungiAlCarrelloAsync(userId , prodottoId);
        }


        [Authorize]
        [HttpDelete("{prodottoId}")]
        public async Task<bool> RimuoviDalCarrello([FromRoute] int prodottoId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new Exception("Utente non trovato");
            return await _carrelloService.RimuoviDalCarrelloAsync(userId, prodottoId);
        }
    }
}
