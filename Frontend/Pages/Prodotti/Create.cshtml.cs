using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace Frontend.Pages.Prodotti
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        [Required(ErrorMessage = "Il nome è obbligatorio")]
        public string NomeProdotto { get; set; }  // Rinominato da 'Nome'

        [BindProperty]
        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        public string Descrizione { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero")]
        public decimal Prezzo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La categoria è obbligatoria")]
        public string Categoria { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "La quantità è obbligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantità deve essere un numero positivo")]
        public int Giacenza { get; set; }  // Rinominato da 'Quantita'

        [BindProperty]
        public IFormFile ImgFile { get; set; }  // Rinominato da 'ImmagineFile'

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(NomeProdotto), "NomeProdotto");
                formData.Add(new StringContent(Descrizione), "Descrizione");
                formData.Add(new StringContent(Prezzo.ToString()), "Prezzo");
                formData.Add(new StringContent(Categoria), "Categoria");
                formData.Add(new StringContent(Giacenza.ToString()), "Giacenza");

                if (ImgFile != null && ImgFile.Length > 0)
                {
                    var fileContent = new StreamContent(ImgFile.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(ImgFile.ContentType);
                    formData.Add(fileContent, "ImgFile", ImgFile.FileName);
                }

                var response = await _httpClient.PostAsync("http://localhost:5150/api/prodotti", formData);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Errore API: {response.StatusCode} - {errorContent}");
                    return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Errore critico: {ex.Message}");
                return Page();
            }
        }
    }
}