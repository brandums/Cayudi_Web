using EPlatformWebApp.Model;

namespace EPlatformWebApp.DTO
{
    public class PostAnswerLikesDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int? BlogPostAnswerID { get; set; }
        public BlogPostAnswer BlogPostAnswer { get; set; }
    }
}
