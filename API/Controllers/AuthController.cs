using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using API.Models.DTO;
using API.Models.Entities;
using API.Models.Services.Application;

using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly IConfiguration _configuration;
    private readonly ITokenBlacklist _tokenBlacklist;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterRequest> registerValidator,
        IConfiguration configuration,
        IValidator<LoginRequestDto> loginValidator,
        ITokenBlacklist tokenBlacklist, 
        UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _configuration = configuration;
        _tokenBlacklist = tokenBlacklist;
        _userManager = userManager;
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
        var ip = GetClientIp();
        

        var validationResult = await _loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Errors = errorMessages });
        }

        var user = await _authService.FindUserAsync(request.Email);

        if (user == null || !await _authService.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized("credenziali non valide.");
        }

        user.Ip = ip;
        await _userManager.UpdateAsync(user);

        var token = GenerateJwtToken(user);

        Response.Cookies.Append("jwtToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.Now.AddHours(1)
        });

        return Ok();
    }
    private string GetClientIp()
    {
        var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrEmpty(ip))
        {
            ip = ip.Split(',')[0].Trim();
        }
        else
        {
            ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        return ip;
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
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
        Console.WriteLine($"Token ricevuto: {token}");

        if (string.IsNullOrEmpty(token) || _tokenBlacklist.IsRevoked(token))
        {
            Console.WriteLine("Token non valido o revocato");
            return Unauthorized();
        }

        Console.WriteLine("Token valido");
        return Ok(true);
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}

