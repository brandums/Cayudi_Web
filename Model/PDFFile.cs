using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.Model
{
    public class PDFFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String? Source { get; set; }
        public String? Path { get; set; }
        public String Tittle { get; set; }
        public String? Description { get; set; } = "application/pdf";
        public bool ReadOnly { get; set; } = false;                                         
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }

        public PDFFile(int courseID, string tittle, string source, string path)
        {
            CourseID = courseID;
            Tittle = tittle;
            Source = source;
            Path = path;
        }
    }
}
