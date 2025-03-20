using API.Models.DTO;
using API.Models.Services.Application;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        private readonly IProdottoService _service;
        private readonly IValidator<ProdottoRequestDto> _requestValidator;
        
        public ProdottiController(
            IProdottoService service,
            IValidator<ProdottoRequestDto> requestValidator)
        {
            _service = service;
            _requestValidator = requestValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProdotti([FromQuery] int page = 1)
        {
            var prodotti = await _service.GetProdottiAsync(page);
            var totaleProdotti = await _service.GetTotalProdottiCountAsync();

            var json = new
            {
                TotalCount = totaleProdotti,
                PaginaCorrente = page,
                TotalePagine = (int)Math.Ceiling(totaleProdotti / 10.0),
                Items = prodotti
            };
            return Ok(json);
        }

        [HttpPost]
        public async Task<IActionResult> CreaProdotto([FromForm] ProdottoRequestDto request)
        {
            // Validazione
            var validationResult = await _requestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Gestione degli errori di validazione
                var errors = validationResult.Errors
                    .Select(e => new { e.ErrorMessage })
                    .ToList();
                return BadRequest(errors);
            }

            try
            {
                // Usa il service per creare il prodotto
                var result = await _service.CreateProdottoAsync(request);
                return Ok("Prodotto creato con successo");
            }
            catch (Exception ex)
            {
                // Gestione di altri errori
                return StatusCode(500, $"Errore interno: {ex.Message}");
            }
        }
    }
}
