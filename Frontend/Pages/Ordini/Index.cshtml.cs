using System.Net.Http.Headers;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Frontend.Pages.Ordini
{
    public class IndexModel : PageModel
    {

        private readonly HttpClient _httpClient;
        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public List<OrdineViewModel> Ordini { get; set; } = new List<OrdineViewModel>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public string? Search { get; set; }
        private const int PageSize = 10;

        public async Task OnGet(string? search , int? pageNumber)
        {
            Search = search;
            CurrentPage = pageNumber ?? 1;

            var url = "http://localhost:5150/api/ordini";
            if (!string.IsNullOrWhiteSpace(search))
            {
                url = $"http://localhost:5150/api/ordini/search?keyword={Uri.EscapeDataString(search)}";
            }

            var token = Request.Cookies["jwtToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(url);
            
            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                Response.Redirect("/AccessoNegato");
                return;
            }

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                
                var ordini = JsonConvert.DeserializeObject<List<OrdineViewModel>>(json);

                if(ordini != null)
                {
                    var totalOrdini = ordini.Count;
                    TotalPages = (int)Math.Ceiling(totalOrdini / (double)PageSize);

                    Ordini = ordini.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var token = Request.Cookies["jwtToken"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"http://localhost:5150/api/ordini/{id}");

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/AccessoNegato");
            }

            if(response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
    }
}
