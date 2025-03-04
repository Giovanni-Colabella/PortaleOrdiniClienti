using Frontend.Customizations.GlobalObjects;
using Frontend.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

namespace Frontend.Pages.Ordini
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public OrdineViewModel Ordine { get; set; } = new OrdineViewModel();

        public List<string> ListValidationErrors { get; set; } = new List<string>();


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5150/api/ordini/{id}");
            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            Ordine = JsonConvert.DeserializeObject<OrdineViewModel>(json);

            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5150/api/ordini/{Ordine.IdOrdine}", Ordine);

            if(response.IsSuccessStatusCode){
                return RedirectToPage("Index");
            }

            string content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                ListValidationErrors.Add("Errore del server: risposta vuota.");
                return Page();
            }

            try
            {
                var validationErrors = JsonConvert.DeserializeObject<ValidationErrors>(content);
                if (validationErrors?.Errors != null && validationErrors.Errors.Count > 0)
                {
                    foreach (var error in validationErrors.Errors)
                    {
                        ListValidationErrors.Add(error);
                    }
                }
                else
                {
                    ListValidationErrors.Add("Errore del server: nessun dettaglio sugli errori.");
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Errore durante la deserializzazione: {ex.Message}");
                ListValidationErrors.Add($"Errore nella risposta del server. {ex.Message}");
            }

            return Page();
        }
    }
}
