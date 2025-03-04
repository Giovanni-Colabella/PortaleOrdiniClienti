using Frontend.Customizations.GlobalObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Clienti
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ClienteViewModel Cliente { get; set; } = new ClienteViewModel();

        public List<string> ListValidationErrors { get; set; } = new();

        private readonly HttpClient _httpClient;
        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Cliente.Nome)) Cliente.Nome = "";
            if (string.IsNullOrEmpty(Cliente.Cognome)) Cliente.Cognome = "";
            if (string.IsNullOrEmpty(Cliente.Email)) Cliente.Email = "";
            if (string.IsNullOrEmpty(Cliente.NumeroTelefono)) Cliente.NumeroTelefono = "";
            if (string.IsNullOrEmpty(Cliente.Indirizzo.Via)) Cliente.Indirizzo.Via = "";
            if (string.IsNullOrEmpty(Cliente.Indirizzo.Citta)) Cliente.Indirizzo.Citta = "";
            if (string.IsNullOrEmpty(Cliente.Indirizzo.CAP)) Cliente.Indirizzo.CAP = "";

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5150/api/clienti", Cliente);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            if (string.IsNullOrWhiteSpace(content))
            {
                ListValidationErrors.Add("Errore del server: risposta vuota.");
                return Page();
            }

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

            return Page();
        }
    }
}
