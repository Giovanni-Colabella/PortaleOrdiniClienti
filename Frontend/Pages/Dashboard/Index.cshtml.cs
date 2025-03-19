using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    public Statistiche? Stats { get; set; } = new();
    private readonly HttpClient _client;

    public IndexModel(HttpClient client)
    {
        _client = client;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var token = Request.Cookies["jwtToken"];
            // if (string.IsNullOrEmpty(token))
            // {
            //     Console.WriteLine("Token JWT non trovato nel cookie.");
            //     return RedirectToPage("/Auth/Login");
            // }

            Console.WriteLine($"Token JWT trovato: {token}");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            foreach (var header in _client.DefaultRequestHeaders)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }

            var dashboardResponse = await _client.GetAsync("http://localhost:5150/api/dashboard");

            if (dashboardResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Token JWT non autorizzato.");
                return RedirectToPage("/AccessoNegato");
            }

            if (!dashboardResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Errore API: {dashboardResponse.StatusCode}");
                return RedirectToPage("/Error");
            }

            var content = await dashboardResponse.Content.ReadAsStringAsync();
            Stats = JsonConvert.DeserializeObject<Statistiche>(content);

            return Page();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
            return RedirectToPage("/Error");
        }
    }

    public class Statistiche
    {
        [JsonProperty("allClienti")]
        public int AllClienti { get; set; }

        [JsonProperty("allOrdini")]
        public int AllOrdini { get; set; }

        [JsonProperty("weeklyClienti")]
        public int WeeklyClienti { get; set; }
    }
}