namespace Frontend.ViewModels
{
    public class ProdottoViewModel
    {
        public string NomeProdotto { get; set; }
        public string Categoria { get; set; }
        public decimal Prezzo { get; set; }
        public int Giacenza { get; set; }
        public DateTime DataInserimento { get; set; }
        public string ImgPath { get; set; }
    }
}
