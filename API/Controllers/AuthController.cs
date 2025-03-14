using API.Models.DTO;
using API.Models.Services.Application;

using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly IConfiguration _configuration;
    private readonly ITokenBlacklist _tokenBlacklist;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequest> registerValidator,
        IConfiguration configuration,
        IValidator<LoginRequestDto> loginValidator,
        ITokenBlacklist tokenBlacklist)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _configuration = configuration;
        _tokenBlacklist = tokenBlacklist;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(new { Errors = errorMessages });
        }
        var result = await _authService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = identityErrors });
        }


        return Ok("Utente registrato!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Errors = errorMessages });
        }

        var user = await _authService.FindUserAsync(request.Email);

        if (user == null || !await _authService.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized("Invalid credentials.");
        }

        var roles = await _authService.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Imposta il token nel cookie direttamente nel controller
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires
        };

        Response.Cookies.Append("jwtToken", tokenString, cookieOptions);

        return Ok(new { Token = tokenString });
    }

    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var token = Request.Cookies["jwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _tokenBlacklist.Add(token);
        }
        
        Response.Cookies.Delete("jwtToken");
        return Ok("Logout effettuato");
    }



    [HttpGet("IsAuthenticated")]
    public IActionResult IsAuthenticated()
    {
        var token = Request.Cookies["jwtToken"];
        
        if (string.IsNullOrEmpty(token) || _tokenBlacklist.IsRevoked(token))
            return Ok(false);

        try 
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
            return Ok(true);
        }
        catch 
        {
            return Ok(false);
        }
    }
}
