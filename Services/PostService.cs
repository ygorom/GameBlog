using Azure;
using GameBlog.Data;
using GameBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBlog.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _db;
        public PostService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetAll()
        {
            return await _db.Posts.ToListAsync();
        }

        public async Task<Post?> GetById(int id)
        {
            return await _db.Posts.FirstOrDefaultAsync(post => post.PostId == id);
        }

        public async Task<Post?> AddPost(Post postObj)
        {
            _db.Posts.Add(postObj);
            await _db.SaveChangesAsync();
            return postObj;
        }

        public async Task<Post?> UpdatePost(Post postObj)
        {
            _db.Posts.Update(postObj);
            await _db.SaveChangesAsync();
            return postObj;
        }

        public async Task<bool> DeletePost(int id)
        {
            var post = await _db.Posts.FirstOrDefaultAsync(post => post.PostId == id);
            if (post == null)
                return false;
            
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}