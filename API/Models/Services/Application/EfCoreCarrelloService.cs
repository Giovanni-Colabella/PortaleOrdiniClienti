using API.Models.DTO;
using API.Models.Entities;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Services.Application;

public class EfCoreCarrelloService : ICarrelloService
{
    private readonly ApplicationDbContext _context;

    public EfCoreCarrelloService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AggiungiAlCarrelloAsync(string idCliente, int prodottoId)
    {
        var prodotto = await _context.Prodotti.FindAsync(prodottoId);
        if (prodotto == null)
            throw new Exception("Prodotto non trovato");

        var carrello = await _context.Carrelli
            .Include(c => c.CarrelloProdotti)
            .FirstOrDefaultAsync(c => c.ClienteId == idCliente);

        if (carrello == null)
        {
            carrello = new Carrello
            {
                ClienteId = idCliente,
                CarrelloProdotti = new List<CarrelloProdotto>()
            };

            _context.Carrelli.Add(carrello);
            await _context.SaveChangesAsync(); // Salviamo il carrello per ottenere l'ID
        }

        // Verifica se il prodotto è già nel carrello
        var carrelloProdotto = carrello.CarrelloProdotti
            .FirstOrDefault(cp => cp.ProdottoId == prodottoId);

        if (carrelloProdotto == null)
        {
            // Se il prodotto non è nel carrello, lo aggiungiamo
            carrelloProdotto = new CarrelloProdotto
            {
                CarrelloId = carrello.Id,
                ProdottoId = prodottoId,
            };

            _context.CarrelloProdotti.Add(carrelloProdotto);
        }
        else
        {
            _context.CarrelloProdotti.Update(carrelloProdotto);
        }

        await _context.SaveChangesAsync();
    }


    public async Task<List<ProdottoResponseDto>> GetArticoliFromCarrelloAsync(string idCliente)
    {
        var carrello = await _context.Carrelli.Include(c => c.CarrelloProdotti)
                                              .ThenInclude(cp => cp.Prodotto)
                                              .FirstOrDefaultAsync(c => c.ClienteId == idCliente);

        if (carrello == null || carrello.CarrelloProdotti.Count == 0)
            return new List<ProdottoResponseDto>();

        return carrello.CarrelloProdotti.Select(cp => new ProdottoResponseDto.Builder()
            .SetId(cp.Prodotto.IdProdotto)
            .SetNomeProdotto(cp.Prodotto.NomeProdotto)
            .SetCategoria(cp.Prodotto.Categoria)
            .SetDescrizione(cp.Prodotto.Descrizione)
            .SetImgPath(cp.Prodotto.ImgPath)
            .SetPrezzo(cp.Prodotto.Prezzo)
            .Build()).ToList();
    }

    public async Task<bool> RimuoviDalCarrelloAsync(string idCliente, int prodottoId)
    {
        // rimuovi dal carrello 
        var carrello = await _context.Carrelli
            .Include(c => c.CarrelloProdotti)
            .FirstOrDefaultAsync(c => c.ClienteId == idCliente);

        if(carrello == null)
            return false;
        
        var carrelloProdotto = carrello.CarrelloProdotti.FirstOrDefault(carrelloProdotto => carrelloProdotto.ProdottoId == prodottoId);

        if(carrelloProdotto == null)
            return false; // Il prodotton non è nel carrello

        _context.CarrelloProdotti.Remove(carrelloProdotto);
        await _context.SaveChangesAsync();

        return true;
    }
}