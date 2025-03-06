using Frontend.Customizations.GlobalObjects;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Newtonsoft.Json;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly HttpClient _httpClient;
    public IndexModel(ILogger<IndexModel> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public Stats? Stats {get; set;} = new Stats();

    public async Task OnGet()
    {
        var response = await _httpClient.GetAsync("http://localhost:5150/api/home");

        if(response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            Stats = JsonConvert.DeserializeObject<Stats>(json);
        }
    }
}
