using EPlatformWebApp.Model;

namespace EPlatformWebApp.DTO
{
    public class PostLikeCreationDTO
    {
        public int UserID { get; set; }
        public int? BlogPostID { get; set; }
    }
}
