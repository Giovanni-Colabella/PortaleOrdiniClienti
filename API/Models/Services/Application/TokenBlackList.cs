using System.Collections.Concurrent;

namespace API.Models.Services.Application;

public class TokenBlacklist : ITokenBlacklist
{
    // Uso questo invece di un normale dictionary perche questa impl. è thread safe
    private readonly ConcurrentDictionary<string, bool> _tokenRevocati = new();
    public void Add(string token)
    {
        _tokenRevocati.TryAdd(token, true);
    }

    public bool IsRevoked(string token)
    {
        return _tokenRevocati.ContainsKey(token);
    }
}
