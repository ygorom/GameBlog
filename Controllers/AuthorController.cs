using System.Security.Claims;
using GameBlog.Models;
using GameBlog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> Post([FromBody] User userObj)
        {
            return Ok(await _userService.AddUser(userObj));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> Patch(int id, [FromBody] User userObj)
        {
            var updateUser = await _userService.GetById(id);
            if (updateUser == null)
                return NotFound();

            string? userId = HttpContext.User.FindFirstValue("id");
            if (userId != "1" && userId != updateUser.UserId.ToString())
                return Unauthorized();

            return Ok(await _userService.UpdateUser(userObj, id));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}