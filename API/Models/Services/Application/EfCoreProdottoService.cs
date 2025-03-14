using API.Models.DTO;
using API.Models.DTO.Mappings;
using API.Models.Services.Infrastructure;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Services.Application
{
    public class EfCoreProdottoService : IProdottoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImagePersister _imagePersister;
        private readonly IWebHostEnvironment _env;

        public EfCoreProdottoService(ApplicationDbContext context,
            IImagePersister imagePersister,
            IWebHostEnvironment env)
        {
            _context = context;
            _imagePersister = imagePersister;
            _env = env;
        }

        public async Task<List<ProdottoResponseDto>> GetProdottiAsync(int page)
        {
            const int pageSize = 10;
            var prodotti = await _context.Prodotti.Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync();

            var prodottiResponseDto = prodotti.Select(p => ProdottoResponseDto.CreateBuilder()
                .SetNomeProdotto(p.NomeProdotto)
                .SetPrezzo(p.Prezzo)
                .SetCategoria(p.Categoria)
                .SetGiacenza(p.Giacenza)
                .SetImgPath(p.ImgPath)
                .Build()
                
            ).ToList();

            return prodottiResponseDto;
        }

        public async Task<int> GetTotalProdottiCountAsync()
        {
            return await _context.Prodotti.CountAsync();
        }

        public async Task<bool> CreateProdottoAsync(ProdottoRequestDto prodottoDto)
        {

            try
            {
                var nuovoProdotto = prodottoDto.ToEntity();

                _context.Prodotti.Add(nuovoProdotto);
                await _context.SaveChangesAsync();

                if (prodottoDto.ImgFile != null)
                {
                    var imgPath = await _imagePersister.SalvaImmagineAsync(nuovoProdotto.IdProdotto, prodottoDto.ImgFile);
                    nuovoProdotto.ImgPath = imgPath;
                    await _context.SaveChangesAsync(); // Aggiorniamo il prodotto con il percorso dell'immagine
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<ProdottoResponseDto> GetProdottoById(int id)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);

            if(prodotto != null)
                return prodotto.ToDto();
            
            throw new KeyNotFoundException($"Prodotto con ID {id} non trovato");
        }

    }
}
