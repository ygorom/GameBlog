using Microsoft.AspNetCore.Mvc;
using GameBlog.Models;
using Microsoft.AspNetCore.Authorization;
using GameBlog.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using GameBlog.Hubs;

namespace GameBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController(IPostService postService, IHubContext<NotificationHub> hub) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly IHubContext<NotificationHub> _hub = hub;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            return Ok(await _postService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            Post? post = await _postService.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> Post([FromBody] PostControllerRequest postObj)
        {
            Post post = new()
            {
                Title = postObj.Title,
                Content = postObj.Content,
                CreatedAt = DateTime.Now,
                AuthorId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int authorId) ? authorId : 0
            };

            Post? savedPost = await _postService.AddPost(post);
            if (savedPost == null)
            {
                return (ActionResult<Post>)BadRequest();
            }
            else
            {
                string message = "new post has been added";
                await _hub.Clients.All.SendAsync("Update!", message, savedPost.Title);
                return (ActionResult<Post>)Ok(new
                {
                    message = "Post added successfully",
                    id = savedPost!.PostId
                });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Post>> Patch(int id, [FromBody] PostControllerRequest postObj)
        {
            Post? post = await _postService.GetById(id);
            if (post == null)
                return NotFound();

            string? userId = HttpContext.User.FindFirstValue("id");
            if (userId != post.AuthorId.ToString())
                return Unauthorized();

            post.Title = postObj.Title;
            post.Content = postObj.Content;
            post.UpdatedAt = DateTime.Now;

            Post? postSaved = await _postService.UpdatePost(post);
            return postSaved == null
                ? (ActionResult<Post>)NotFound()
                : (ActionResult<Post>)Ok(new
                {
                    message = "Post updated successfully",
                    id = postSaved!.PostId
                });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _postService.DeletePost(id))
                return NotFound();

            return Ok(new
            {
                message = "Post deleted successfully",
                id
            });
        }
    }
}