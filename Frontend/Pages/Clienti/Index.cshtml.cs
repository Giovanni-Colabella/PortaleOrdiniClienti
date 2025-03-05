using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyProject.Pages.Clienti
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private const int PageSize = 10;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<ClienteViewModel>? Clienti { get; set; } = new List<ClienteViewModel>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Search { get; set; }

        public async Task OnGetAsync(string? search, int pageNumber = 1)
        {
            CurrentPage = pageNumber;
            Search = search;

            // url di base 
            string url = "http://localhost:5150/api/clienti";
            // Se search non contiene stringhe vuote o spazi bianchi
            if (!string.IsNullOrWhiteSpace(search))
            {
                // costruisci l'url 
                url = $"http://localhost:5150/api/clienti/search?keyword={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                
                var json = await response.Content.ReadAsStringAsync();
                var clienti = JsonSerializer.Deserialize<List<ClienteViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (clienti != null)
                {
                    var totalItems = clienti.Count;
                    TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
                    Clienti = clienti.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }
            }
            else
            {
                Clienti = new List<ClienteViewModel>(); // Evita null reference
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5150/api/clienti/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage(new { search = Search, pageNumber = CurrentPage });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Errore durante l'eliminazione del cliente.");
                return Page();
            }
        }
    }
}
