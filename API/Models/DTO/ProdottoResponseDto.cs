namespace API.Models.DTO
{
    public record ProdottoResponseDto
    {
        public string NomeProdotto { get; init; }
        public string Categoria { get; init; }
        public decimal Prezzo { get; init; }
        public int Giacenza { get; init; }
        public string ImgPath { get; init; } = $"";

        private ProdottoResponseDto() {}
        private ProdottoResponseDto(Builder builder)
        {
            NomeProdotto = builder.NomeProdotto;
            Categoria = builder.Categoria;
            Prezzo = builder.Prezzo;
            Giacenza = builder.Giacenza;
            ImgPath = builder.ImgPath;
        }

        public static Builder CreateBuilder() => new Builder();

        public class Builder
        {
            public string NomeProdotto { get; private set; }
            public string Categoria { get; private set; } 
            public decimal Prezzo { get; private set; }
            public int Giacenza { get; private set; }
            public string ImgPath { get; private set; } 

            public Builder SetNomeProdotto(string nomeProdotto) { NomeProdotto = nomeProdotto; return this; }
            public Builder SetCategoria(string categoria) { Categoria = categoria; return this; }
            public Builder SetPrezzo(decimal prezzo) { Prezzo = prezzo; return this; }
            public Builder SetGiacenza(int giacenza) { Giacenza = giacenza; return this; }
            public Builder SetImgPath(string imgPath) { ImgPath = imgPath; return this; }

            public ProdottoResponseDto Build() => new ProdottoResponseDto(this);
        }
    }
}
