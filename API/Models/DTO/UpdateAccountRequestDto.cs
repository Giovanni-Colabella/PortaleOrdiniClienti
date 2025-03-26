namespace API.Models.DTO;

public record class UpdateAccountRequestDto
{
    public string UserId { get; init; }
    public string Nome { get; init; }
    public string Cognome { get; init; }
    public string Email { get; init; }  
}
