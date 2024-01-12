using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoListApp.Configs;
using TodoListApp.Dtos;
using TodoListApp.Persistance;
using TodoListApp.Persistance.Entities;
using TodoListApp.Persistance.Entities.Enums;
using TodoListApp.Services;

namespace TodoListApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly TodoListAppDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;

    public AuthController(
        TodoListAppDbContext dbContext,
        IEmailService emailService,
        ITokenService tokenService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            ModelState.AddModelError("*", "Email or password is not correct");

            return BadRequest(ModelState);
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
        {
            ModelState.AddModelError("*", "Email or password is not correct");

            return BadRequest(ModelState);
        }

        if (!user.IsActivated)
        {
            ModelState.AddModelError("*", "Account isn't activated yet");
        }


        var jwtToken = _tokenService.GenerateToken(user);

        return Ok(jwtToken);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (await _dbContext.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            ModelState.AddModelError("Email", "Email is already used");

            return BadRequest(ModelState);
        }

        var hashedPass = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Email,
            Password = hashedPass,
            ActivationExpireDate = DateTime.UtcNow.AddHours(2),
            Role = Role.User
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        var scheme = HttpContext.Request.Scheme;
        var domainName = HttpContext.Request.Host.Host;
        var port = HttpContext.Request.Host.Port;
        var endpoint = Url.RouteUrl("auth-activate", new { userId = user.Id });

        //builder pattern
        var uriBuilder = new UriBuilder(scheme, domainName, port.Value, endpoint);
        var uri = uriBuilder.Uri.ToString();


        _emailService.SendEmail(user.Email, "Pls activate your account", uri);

        return Ok();
    }


    [HttpGet("activate/{userId}", Name = "auth-activate")]
    public async Task<IActionResult> Activate([FromRoute] int userId)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.ActivationExpireDate > DateTime.UtcNow);
        if (user == null)
        {
            return BadRequest("User either isn't found or activation is expired");
        }

        user.IsActivated = true;

        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
