using API.Models;
using API.Models.Services.Application;
using API.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<ClienteDto> _clienteDtoValidator;
        private readonly IClienteService _clienteService;

        public ClientiController(
            ApplicationDbContext context,
            IValidator<ClienteDto> clienteDtoValidator,
            IClienteService clienteService)
        {
            _context = context;
            _clienteDtoValidator = clienteDtoValidator;
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClienti()
        {
            List<ClienteDto> clienti = await _clienteService.GetClientiAsync();
            return Ok(clienti);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCliente(int id)
        {
            ClienteDto cliente = await _clienteService.GetCliente(id);

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente(ClienteDto clienteDto)
        {
            var validationResult = await _clienteDtoValidator.ValidateAsync(clienteDto);
            if(!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errorMessages });
            }

            var result = await _clienteService.CreateClienteAsync(clienteDto);
            
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCliente(int id, [FromBody] ClienteDto clienteDto)
        {

            var validationResult = await _clienteDtoValidator.ValidateAsync(clienteDto);

            if (!validationResult.IsValid)
            {
                // Ritorna un BadRequest con solo i messaggi di errore.
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { Errors = errorMessages });
            }


            var updatedCliente = await _clienteService.UpdateClienteAsync(id, clienteDto);
            return Ok(updatedCliente);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _clienteService.DeleteClienteAsync(id);
            return Ok(result);
        }
    }
}
