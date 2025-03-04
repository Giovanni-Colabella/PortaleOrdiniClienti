using API.Models.DTO;
using API.Models.Entities;
using API.Services;

using Microsoft.EntityFrameworkCore;

namespace API.Models.Services.Application;

public class EfCoreOrdineService : IOrdineService
{

    private readonly ApplicationDbContext _context;
    public EfCoreOrdineService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrdineDto>> GetOrdiniAsync()
    {
        List<Ordine> ordini = await _context.Ordini.Include(o => o.Cliente).OrderByDescending(o => o.IdOrdine).ToListAsync();
        List<OrdineDto> ordiniDto = ordini.Select(ordine => OrdineDto.FromEntity(ordine)).ToList();

        return ordiniDto;
    }


    public async Task<OrdineDto> GetOrdineAsync(int id)
    {
        Ordine ordine = await  _context.Ordini
                                .Include(o => o.Cliente) 
                                .FirstOrDefaultAsync(o => o.IdOrdine == id);
        if (ordine == null)
        {
            throw new KeyNotFoundException("Ordine non trovato");
        }

        return OrdineDto.FromEntity(ordine);

    }

    public async Task<bool> CreateOrdineAsync(OrdineDto ordineDto)
    {
        var altroOrdine = await _context.Ordini.FirstOrDefaultAsync(o => o.IdOrdine == ordineDto.IdOrdine);
        if (altroOrdine != null)
        {
            return false;
        }

        Ordine ordine = new Ordine 
        {
            IdOrdine = ordineDto.IdOrdine,
            TotaleOrdine = ordineDto.TotaleOrdine,
            DataOrdine = ordineDto.DataOrdine,
            Stato = ordineDto.Stato,
            MetodoPagamento = ordineDto.MetodoPagamento
        };

        _context.Ordini.Add(ordine);
        await _context.SaveChangesAsync();

        return true;

    }


    public async Task<bool> DeleteOrdineAsync(int id)
    {
        var ordine = await _context.Ordini.FindAsync(id);
        if (ordine == null)
        {
            return false;
        }

        _context.Ordini.Remove(ordine);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<bool> UpdateOrdineAsync(int id, OrdineDto ordineDto)
    {
        var ordine = await _context.Ordini.FindAsync(id);
        if (ordine == null)
        {
            return false;
        }

        ordine.ClienteId = ordineDto.ClienteId;
        ordine.TotaleOrdine = ordineDto.TotaleOrdine;
        ordine.DataOrdine = ordineDto.DataOrdine;
        ordine.Stato = ordineDto.Stato;
        ordine.MetodoPagamento = ordineDto.MetodoPagamento;

        await _context.SaveChangesAsync();

        return true;
    }

    
}
