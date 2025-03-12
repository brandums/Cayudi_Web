using EPlatformWebApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.DTO
{
    public class PostLikeDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int? BlogPostID { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
