using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Auth
{
    public class AccountManagerModel : PageModel
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public AccountManagerModel(HttpClient client,
            IConfiguration config)
        {
            _client = client;
            _config = config;
        }


        public ApplicationUserViewModel User { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var baseUrl = _config["ApiUrl"];

            var token = Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"{baseUrl}api/auth/GetUser");

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Auth/Login");
            }

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                User = JsonConvert.DeserializeObject<ApplicationUserViewModel>(json);
            }

            return Page();
        }
    }
    public class ApplicationUserViewModel
    {
        public string Id { get; set; } 
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
    }
}
