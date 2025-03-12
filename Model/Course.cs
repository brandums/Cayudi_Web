using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EPlatformWebApp.Model
{
    public class Course
    {
        #region
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Tittle { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public int CourseCategoryID { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public float? Price { get; set; }
        public int TrainerID { get; set; }
        public DateTime StartDate { get; set; }
        public string? ImagePath { get; set; } = string.Empty;
        public string? BannerPath { get; set; } = string.Empty;

        [Required]   
        public int? DurationTime { get; set; } = 30;

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }
        [ForeignKey("TrainerID")]
        public virtual User Trainer { get; set; }
        #endregion
    }
}
