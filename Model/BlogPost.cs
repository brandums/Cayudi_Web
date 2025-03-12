using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class BlogPost : Post
    {
        #region "Attributes"
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BlogID { get; set; }
        [ForeignKey("BlogID")]
        public virtual Blog Blog { get; set; }
        #endregion
    }
}
