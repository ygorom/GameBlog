using System.Text.Json.Serialization;

namespace GameBlog.Models
{
    public class User
    {
        [JsonIgnore]
        public int UserId {get; set;}
        public required string Username {get; set;}
        public required string Password {get; set;}
    }
}