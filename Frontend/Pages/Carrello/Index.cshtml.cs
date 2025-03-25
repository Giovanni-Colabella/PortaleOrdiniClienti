using System.Net;
using System.Net.Http.Headers;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Carrello
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public IndexModel(HttpClient httpClient,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public string GetImmagineUrl(string nomeFile)
        {
            if (string.IsNullOrWhiteSpace(nomeFile))
                return _config["ApiUrl"]; // Immagine di default

            var baseUrl = _config["ApiUrl"];
            
            return $"{baseUrl}{nomeFile}";
        }
        
        public List<ProdottoViewModel> Articoli { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var jwtToken = Request.Cookies["jwtToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var url = _config["ApiUrl"];
            var response = await _httpClient.GetAsync($"{url}api/carrello");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Articoli = JsonConvert.DeserializeObject<List<ProdottoViewModel>>(content) ?? new();
            }

            if(response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Auth/Login");
            }

            if(!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }
    }
}
