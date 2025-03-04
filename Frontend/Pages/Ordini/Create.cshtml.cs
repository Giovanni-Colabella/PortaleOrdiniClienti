using Frontend.Customizations.GlobalObjects;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Ordini
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<string> ListValidationErrors { get; set; } = new();

        [BindProperty]
        public OrdineViewModel Ordine { get; set; } = new OrdineViewModel();

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5150/api/ordini", Ordine);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            string content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                ListValidationErrors.Add("Errore del server: risposta vuota.");
                return Page();
            }

            var validationErrors = JsonConvert.DeserializeObject<ValidationErrors>(content);

            if(validationErrors?.Errors != null && validationErrors.Errors.Count > 0)
            {
                foreach (var error in validationErrors.Errors)
                {
                    ListValidationErrors.Add(error);
                }
            } else
            {
                ListValidationErrors.Add("Errore del server: nessun dettaglio sugli errori.");
            }

            return Page();
        }
    }
}
