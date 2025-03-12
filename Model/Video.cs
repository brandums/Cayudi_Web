using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EPlatformWebApp.Model
{
    public class Video
    {
        #region "Attributes"
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String? Source { get; set; }
        public String? Path { get; set; }
        public String Tittle { get; set; }
        public String? Description { get; set; } = "video/mp4";
        public int? Order { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }
        #endregion

        public Video(int courseID, string tittle, string source, string path)
        {
            CourseID = courseID;
            Tittle = tittle;
            Source = source;
            Path = path;
        }
    }
}
