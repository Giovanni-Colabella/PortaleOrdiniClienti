using System.Net.Http.Headers;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Prodotti
{
    public class DetailModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public DetailModel(HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        
        [BindProperty]
        public ProdottoViewModel Prodotto { get; set; } = new();

        public string GetImmagineUrl(string nomeFile)
        {
            if (string.IsNullOrWhiteSpace(nomeFile))
                return _configuration["ApiUrl"]; // Immagine di default

            var baseUrl = _configuration["ApiUrl"];
            
            return $"{baseUrl}{nomeFile}";
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var token = Request.Cookies["jwtToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"http://localhost:5150/api/prodotti/{id}");


            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/AccessoNegato");
            }
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            Prodotto = JsonConvert.DeserializeObject<ProdottoViewModel>(jsonResponse) ?? new ProdottoViewModel();


            return Page();
        }
    }


}
