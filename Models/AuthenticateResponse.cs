namespace GameBlog.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticateResponse(User author, string token)
        {
            Id = author.UserId;
            Username = author.Username;
            Token = token;
        }
    }
}