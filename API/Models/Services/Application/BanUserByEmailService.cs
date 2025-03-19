using API.Models.DTO;
using API.Models.Entities;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Models.Services.Application;

public class BanUserByEmailService : IUtenteBloccatoService
{
    private readonly ApplicationDbContext _context;
    public BanUserByEmailService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> BanUser(BanUserDto banUserDto)
    {
        var cliente = await _context.Clienti
                            .FirstOrDefaultAsync(c => c.Email == banUserDto.Email);

        if (cliente is null)
            return false; // Il cliente non è stato trovato

        var alreadyBanned = await _context.UtentiBloccati
                                    .AnyAsync( c => c.Email == banUserDto.Email);

        if (alreadyBanned ) return false; // Utente già bannato


        var nuovoBan = new UtenteBloccato {
            IdCliente = cliente.Id,
            Email = cliente.Email,
            NomeCompleto = cliente.Nome +" " +cliente.Cognome
        };

        await _context.AddAsync(nuovoBan);
        await _context.SaveChangesAsync();

        return true;
            
    }

    public async Task<bool> IsUserBanned(string email )
    {
        return await _context.UtentiBloccati.AnyAsync(u => u.Email == email);
    }
}
