using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.Model
{
    public class CourseTest
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
    }
}
