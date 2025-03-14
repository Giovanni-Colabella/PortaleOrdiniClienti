namespace API.Models.Services.Application;

public interface ITokenBlacklist
{
    void Add(string token);
    bool IsRevoked(string token);
}
