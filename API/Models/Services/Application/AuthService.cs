using API.Models.DTO;
using API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Models.Services.Application
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Nome = request.Nome,
                Cognome = request.Cognome
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Verifica esistenza ruolo e creazione se necessario
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                // Aggiungi ruolo all'utente
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                        Console.WriteLine($"Errore ruolo: {error.Description}");
                }
            }

            return result;
        }

        public async Task<ApplicationUser> FindUserAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> UpdateAccountAsync(UpdateAccountRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if ( user == null ) return false; // Utente non trovato

            user.Nome = dto.Nome;
            user.Cognome = dto.Cognome;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;

        }

        public async Task<bool> UpdatePasswordAsync(UpdatePasswordRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if(user == null ) return false; // Account non trovato

            if(!await _userManager.CheckPasswordAsync(user, dto.PasswordCorrente))
                return false;

            var result = await _userManager.ChangePasswordAsync(user, dto.PasswordCorrente, dto.NuovaPassword);
            return true; 
        }

        public ApplicationUserDto GetAppUser(ApplicationUser user)
        {
            return new ApplicationUserDto {
                UserId = user.Id,
                Nome = user.Nome,
                Cognome = user.Cognome,
                Email = user.Email
            };
        }
    }
}
