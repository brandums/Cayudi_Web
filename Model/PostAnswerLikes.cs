using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class PostAnswerLikes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public int? BlogPostAnswerID { get; set; }
        [ForeignKey("BlogPostAnswerID")]
        public virtual BlogPostAnswer? BlogPostAnswer { get; set; }
    }
}
