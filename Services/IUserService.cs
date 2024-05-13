using GameBlog.Models;

namespace GameBlog.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(int id);
        Task<User?> UpdateUser(User userObj, int id);
        Task<User?> AddUser(User userObj);
    } 
}