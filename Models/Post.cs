using System.Text.Json.Serialization;

namespace GameBlog.Models
{
    public class Post
    {
        public int PostId {get; set;}
        public required string Title {get; set;}
        public required string Content {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime? UpdatedAt {get; set;}
        public int AuthorId {get; set;}
    }
}