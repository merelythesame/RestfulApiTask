using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
	[Route("api")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private bool UsersExist() => StaticData.Users != null && StaticData.Users.Any();

		[Authorize(Roles ="Admin")]
		[HttpGet("users")]
		public IActionResult GetAllUsers()
		{
			if (!UsersExist())
			{
				return NotFound("No users found.");
			}

			return Ok(StaticData.Users);
		}

		[Authorize]
		[HttpGet("user")]
		public IActionResult GetUserById([FromQuery] int id)
		{
			if (!UsersExist())
			{
				return NotFound("No users found.");
			}

			var user = StaticData.Users.FirstOrDefault(x => x.Id == id);

			if (user == null)
			{
				return NotFound($"User with id {id} is not found.");
			}

			return Ok(user);
		}

		[Authorize]
		[HttpPost("user/add")]
		public IActionResult AddUser([FromBody] User newUser)
		{
			if (newUser == null)
			{
				return BadRequest("User data is required.");
			}

			if (string.IsNullOrWhiteSpace(newUser.Name) || string.IsNullOrWhiteSpace(newUser.Email))
			{
				return BadRequest("Name and Email are required.");
			}

			newUser.Id = StaticData.Users.Count + 1;
			StaticData.Users.Add(newUser);

			return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
		}

		[Authorize(Roles ="User")]
		[HttpPatch("user/update")]
		public IActionResult UpdateUserById([FromQuery] int id, [FromBody] User updatedUser)
		{
			if (!UsersExist())
			{
				return NotFound("No users found.");
			}

			var target = StaticData.Users.FirstOrDefault(x => x.Id == id);

			if (target == null)
			{
				return NotFound($"User with id {id} is not found.");
			}

			if (string.IsNullOrWhiteSpace(updatedUser.Name) && string.IsNullOrWhiteSpace(updatedUser.Email))
			{
				return BadRequest("At least one value to update (Name or Email) is required.");
			}

			if (!string.IsNullOrWhiteSpace(updatedUser.Name))
			{
				target.Name = updatedUser.Name;
			}

			if (!string.IsNullOrWhiteSpace(updatedUser.Email))
			{
				target.Email = updatedUser.Email;
			}

			return Ok(target);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("user/delete")]
		public IActionResult DeleteUser([FromQuery] int id)
		{
			if (!UsersExist())
			{
				return NotFound("No users found.");
			}

			var findUser = StaticData.Users.FirstOrDefault(x => x.Id == id);

			if (findUser == null)
			{
				return NotFound($"User with id {id} is not found.");
			}

			StaticData.Users.Remove(findUser);
			return NoContent();
		}

	}
}
