using GameBlog.Models;

namespace GameBlog.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAll();
        Task<Post?> GetById(int id);
        Task<Post?> AddPost(Post postObj);
        Task<Post?> UpdatePost(Post postObj);
        Task<bool> DeletePost(int id);
    }
}