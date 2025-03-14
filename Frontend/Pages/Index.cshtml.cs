using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Prodotti
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IndexModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [BindProperty]
        public List<ProdottoViewModel> Prodotti { get; set; } = new List<ProdottoViewModel>();

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public string? Search { get; set; }
        private const int PageSize = 10;

        // Classe per deserializzare la risposta API
        public class ProdottoResponse
        {
            public int TotalCount { get; set; }
            public int PaginaCorrente { get; set; }
            public int TotalePagine { get; set; }
            public List<ProdottoViewModel>? Items { get; set; }
        }

        // Metodo helper per generare l'URL dell'immagine
        public string GetImmagineUrl(string nomeFile)
        {
            if (string.IsNullOrWhiteSpace(nomeFile))
                return _configuration["ApiUrl"]; // Immagine di default

            var baseUrl = _configuration["ApiUrl"];
            
            return $"{baseUrl}{nomeFile}";
        }

        public string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // Se la lunghezza desiderata è troppo piccola per i "...", restituisci il testo originale
            if (maxLength <= 3)
                return text.Length > maxLength ? text.Substring(0, maxLength) : text;

            // Tronca a (maxLength - 3) caratteri e aggiunge "..."
            return text.Length > maxLength ? text.Substring(0, maxLength - 3) + "..." : text;
        }


        public async Task<IActionResult> OnGetAsync(int? pageNumber, string? search)
        {
            CurrentPage = pageNumber ?? 1;
            Search = search;

            var url = $"http://localhost:5150/api/prodotti";

            if (!string.IsNullOrWhiteSpace(search))
            {
                url = $"http://localhost:5150/api/prodotti/search?keyword={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetAsync($"{url}?page={CurrentPage}&pageSize={PageSize}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/AccessoNegato");
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProdottoResponse>(json);

                if (result?.Items != null)
                {
                    Prodotti = result.Items;
                    TotalPages = result.TotalePagine;
                }
                else
                {
                    Prodotti = new List<ProdottoViewModel>();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Errore durante il recupero dei dati.");
            }

            return Page();
        }
    }
}