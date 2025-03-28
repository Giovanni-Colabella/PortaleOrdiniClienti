using API.Models.DTO;
using API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Models.Services.Application;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterRequest request);
    Task<ApplicationUser> FindUserAsync(string email);
    Task<IList<string>> GetRolesAsync(ApplicationUser user);
    ApplicationUserDto GetAppUser(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<bool> UpdateAccountAsync(UpdateAccountRequestDto dto);
    Task<bool> UpdatePasswordAsync(UpdatePasswordRequestDto dto);
}
