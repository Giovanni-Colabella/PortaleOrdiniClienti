using System.Net;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

namespace Frontend.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public class LoginInputModel
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public List<string> ListValidationErrors { get; set; } = new();

        public class ValidationErrors
        {
            [JsonProperty("errors")]
            public List<string> Errors { get; set; } = new();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var requestData = new StringContent(JsonConvert.SerializeObject(Input), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5150/api/Auth/login", requestData);

            if (response.IsSuccessStatusCode)
            {
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    cookies.ToList().ForEach(c => Response.Headers.Append("Set-Cookie", c));
                }

                return RedirectToPage("/Index");
            }


            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var validationErrors = JsonConvert.DeserializeObject<ValidationErrors>(errorContent);

                    if (validationErrors?.Errors != null)
                    {
                        ListValidationErrors.AddRange(validationErrors.Errors);
                    }
                    else
                    {
                        ListValidationErrors.Add("Errore generico dal server");
                    }
                }
                catch
                {
                    ListValidationErrors.Add("Errore nel formato della risposta");
                }
                return Page();
            }

            return Page();
        }
    }
}
