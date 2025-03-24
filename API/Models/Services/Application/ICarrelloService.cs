using API.Models.DTO;

namespace API.Models.Services.Application;

public interface ICarrelloService
{
    Task AggiungiAlCarrelloAsync(int idCliente, int prodottoId);
    Task<List<ProdottoResponseDto>> GetArticoliFromCarrelloAsync(int idCliente);
}
