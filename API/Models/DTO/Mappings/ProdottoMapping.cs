using API.Models.Entities;

namespace API.Models.DTO.Mappings;

public static class ProdottoMapping
{   
    public static Prodotto ToEntity(this ProdottoRequestDto prodottoRequestDto)
    {
        return new Prodotto 
        {
            NomeProdotto = prodottoRequestDto.NomeProdotto,
            Categoria = prodottoRequestDto.Categoria,
            Descrizione = prodottoRequestDto.Descrizione,
            Prezzo = prodottoRequestDto.Prezzo,
            Giacenza = prodottoRequestDto.Giacenza
        };
    }

    public static ProdottoResponseDto ToDto(this Prodotto prodotto)
    {
        return ProdottoResponseDto.CreateBuilder().SetNomeProdotto(prodotto.NomeProdotto)
                                                            .SetPrezzo(prodotto.Prezzo)
                                                            .SetCategoria(prodotto.Categoria)
                                                            .SetDescrizione(prodotto.Descrizione)
                                                            .SetGiacenza(prodotto.Giacenza)
                                                            .SetImgPath(prodotto.ImgPath)
                                                            .Build();
    }
}
