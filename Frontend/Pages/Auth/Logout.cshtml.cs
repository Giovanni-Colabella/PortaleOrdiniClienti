using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        
        public LogoutModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _httpClient.PostAsync($"{_config["ApiUrl"]}api/auth/logout", null);

            return RedirectToPage("/Index");
        }
    }
}
