using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class UserCourse
    {
        #region "Attributes"
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool? IsOnline { get; set; } = false;
        public bool? IsOppened { get; set; } = false;
        public bool? IsFinished { get; set; } = false;
        public int? Rating { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
       
        #endregion
    }
}
