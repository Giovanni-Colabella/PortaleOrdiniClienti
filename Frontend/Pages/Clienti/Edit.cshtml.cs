using Frontend.Customizations.GlobalObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Clienti
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public ClienteViewModel Cliente { get; set; } = new ClienteViewModel();
        public List<string> ListValidationErrors { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            

            var response = await _httpClient.GetAsync($"http://localhost:5150/api/clienti/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            Cliente = JsonConvert.DeserializeObject<ClienteViewModel>(json);

            return Page();
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

            var response = await _httpClient.PutAsJsonAsync($"http://localhost:5150/api/clienti/{Cliente.Id}", Cliente);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

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
                ListValidationErrors.Add("Errore nella risposta del server.");
            }

            return Page();
        }
    }

    // Classe per mappare il JSON degli errori
    
}
