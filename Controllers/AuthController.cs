using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
	[Route("api")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public AuthController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel model)
		{
			var member = StaticData.Members.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

			if (member == null)
			{
				return Unauthorized("Invalid credentials");
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var secretKey = _configuration.GetValue<string>("JWTSetting:SecretKey"); 
			var key = Encoding.UTF8.GetBytes(secretKey);


			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
				new Claim(ClaimTypes.Name, member.Username),
				new Claim(ClaimTypes.Role, member.Role) 
				}),
				Issuer = _configuration["JWTSetting:Issuer"], 
				Audience = _configuration["JWTSetting:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return Ok(new { token = tokenHandler.WriteToken(token) });
		}
	}
}
