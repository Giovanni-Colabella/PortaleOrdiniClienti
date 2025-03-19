namespace API.Models.DTO;

public record class BanUserDto
{
    public string Email { get; init; } = "";
    public string? Motivazione { get; init; }
}
