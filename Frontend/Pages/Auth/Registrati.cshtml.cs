using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

namespace Frontend.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public class RegisterInputModel
        {
            [Display(Name = "Nome")]
            public string Nome { get; set; } = "";

            [Display(Name = "Cognome")]
            public string Cognome { get; set; } = "";

            [Display(Name = "Email")]
            public string Email { get; set; } = "";

            [Display(Name = "Password")]
            public string Password { get; set; } = "";
        }

        public List<string> ListValidationErrors { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Input.Nome)) Input.Nome = "";
            if (string.IsNullOrEmpty(Input.Cognome)) Input.Cognome = "";
            if (string.IsNullOrEmpty(Input.Email)) Input.Email = "";
            if (string.IsNullOrEmpty(Input.Password)) Input.Password = "";

            var requestData = new
            {
                Input.Nome,
                Input.Cognome,
                Input.Email,
                Input.Password
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("http://localhost:5150/api/Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();

            try
            {
                var validationErrors = JsonConvert.DeserializeObject<ValidationErrors>(errorContent);

                if (validationErrors?.Errors != null)
                {
                    foreach (var errorMessage in validationErrors.Errors)
                    {
                        // Aggiungi a entrambe le strutture
                        ListValidationErrors.Add(errorMessage);
                        ModelState.AddModelError(string.Empty, errorMessage);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Errore generico del server");
                    ListValidationErrors.Add("Errore generico del server");
                }
            }
            catch (JsonException)
            {
                ListValidationErrors.Add("Errore nel formato della risposta");
            }

            return Page();
        }
    }

    // Classe per mappare il JSON degli errori
    public class ValidationErrors
    {
        // [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();
    }
}
