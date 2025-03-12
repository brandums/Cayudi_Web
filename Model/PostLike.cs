using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class PostLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public int? BlogPostID { get; set; }
        [ForeignKey("BlogPostID")]
        public virtual BlogPost? BlogPost { get; set; }
    }
}
