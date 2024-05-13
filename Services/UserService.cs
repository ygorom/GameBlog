using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameBlog.Data;
using GameBlog.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameBlog.Services
{
    public class UserService(IOptions<AppSettings> appSettings, AppDbContext _db) : IUserService
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly AppDbContext db = _db;

        public async Task<IEnumerable<User>> GetAll()
        {
            return await db.Authors.ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await db.Authors.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> UpdateUser(User userObj, int id)
        {
            var obj = await db.Authors.FirstOrDefaultAsync(c => c.UserId == id);
            if (obj != null)
            {
                obj.Username = userObj.Username;
                obj.Password = userObj.Password;
                db.Authors.Update(obj);
                return await db.SaveChangesAsync() > 0 ? userObj : null;
            }

            return null;
        }

        public async Task<User?> AddUser(User userObj)
        {
            await db.Authors.AddAsync(userObj);
            return await db.SaveChangesAsync() > 0 ? userObj : null;
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var author = await db.Authors.SingleOrDefaultAsync(x => x.Username == model.Username && 
                                                                    x.Password == model.Password);
            if (author == null) return null;

            var token = await GenerateJwtToken(author);
            return new AuthenticateResponse(author, token);
        }

        private async Task<string> GenerateJwtToken(User author)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([new Claim("id", author.UserId.ToString())]),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                                SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);;
            });
            return tokenHandler.WriteToken(token);
        }
    }
}