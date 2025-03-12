using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.Model
{
    public class UserImage
    {
        public int ID { get; set; }
        public String? Source { get; set; }
        public String? Path { get; set; }

        public String? Tittle { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
