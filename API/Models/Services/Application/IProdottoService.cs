using API.Models.DTO;

namespace API.Models.Services.Application
{
    public interface IProdottoService
    {
        Task<List<ProdottoResponseDto>> GetProdottiAsync(int page);
        Task<int> GetTotalProdottiCountAsync();
        Task<bool> CreateProdottoAsync(ProdottoRequestDto requestDto);
    }
}
