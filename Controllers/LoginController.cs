using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollegeApp.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;
    public LoginController(IConfiguration config)
    {
        _config = config;
    }
    [HttpPost]
    public ActionResult Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Please provide username and password");
        }

        LoginResponseDto responseDto = new LoginResponseDto()
        {
            Username = model.Username,
        };

        if (model.Username == "ashish" && model.Password == "123456")
        {
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("JWTSecretKey"));
            var tokenHandeler = new JwtSecurityTokenHandler();// In this token handler we need to put the data.
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //username
                    new Claim(ClaimTypes.Name,model.Username),
                    //role
                    new Claim(ClaimTypes.Role,"Admin"),
                    
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandeler.CreateToken(tokenDescriptor);
            responseDto.Token = tokenHandeler.WriteToken(token);
            
        }
        else
        {
            return Ok("Username or password is incorrect");
        }
        return Ok(responseDto);
    }
}
