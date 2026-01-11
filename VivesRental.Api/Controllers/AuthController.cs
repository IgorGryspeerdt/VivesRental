using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VivesRental.Services.Model.Requests;
using VivesRental.Api.Services;

namespace VivesRental.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AuthenticationManager _authenticationManager;

    public AuthController(UserManager<IdentityUser> userManager, AuthenticationManager authenticationManager)
    {
        _userManager = userManager;
        _authenticationManager = authenticationManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid)
        {
            return Unauthorized(new { error = "Invalid credentials" });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _authenticationManager.GenerateJwtToken(user, roles);

        // parse token to get expiration
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var expires = jwtToken.ValidTo;

        return Ok(new { token, expires });
    }
}
