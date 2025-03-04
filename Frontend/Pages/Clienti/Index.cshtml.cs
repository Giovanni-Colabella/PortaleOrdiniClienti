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

        public async Task OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            var response = await _httpClient.GetAsync("http://localhost:5150/api/clienti");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var clienti = JsonSerializer.Deserialize<List<ClienteViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (clienti != null)
                {
                    var totalItems = clienti.Count; // Use the count from the API response
                    TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize); // Calculate the total pages based on the API response

                    Clienti = clienti.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList(); // Get the clients for the current page
                }
            }
        }

        // This method is for deleting a client
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"http://localhost:5150/api/clienti/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Errore durante l'eliminazione del cliente.");
                return Page();
            }
        }
    }
}
