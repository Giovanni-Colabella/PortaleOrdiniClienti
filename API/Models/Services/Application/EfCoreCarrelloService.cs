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

    public async Task AggiungiAlCarrelloAsync(int idCliente, int prodottoId)
    {
        var prodotto = await _context.Prodotti.FindAsync(prodottoId);

        if(prodotto == null)
            throw new Exception("Prodotto non trovato");
        
        var carrello = await _context.Carrelli.Include( c => c.CarrelloProdotti)
                                                .FirstOrDefaultAsync( c => c.ClienteId == idCliente);
        
        if(carrello == null)
        {
            carrello = new Carrello 
            {
                ClienteId = idCliente,
                CarrelloProdotti = new List<CarrelloProdotto>() 
            };

            _context.Carrelli.Add(carrello);    
            await _context.SaveChangesAsync();
        }

        var carrelloProdotto = new CarrelloProdotto
        {
            CarrelloId = carrello.Id,
            ProdottoId = prodottoId,
        };

        carrello.CarrelloProdotti.Add(carrelloProdotto);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProdottoResponseDto>> GetArticoliFromCarrelloAsync(int idCliente)
    {
        var carrello = await _context.Carrelli.Include( c => c.CarrelloProdotti)
                                                .ThenInclude( cp => cp.Prodotto)
                                                .FirstOrDefaultAsync( c => c.ClienteId == idCliente);

        return carrello.CarrelloProdotti.Select( cp => new ProdottoResponseDto.Builder().SetId(cp.Prodotto.IdProdotto)
                                                                                          .SetNomeProdotto(cp.Prodotto.NomeProdotto)
                                                                                          .SetPrezzo(cp.Prodotto.Prezzo)
                                                                                          .Build()).ToList();


    }
}
