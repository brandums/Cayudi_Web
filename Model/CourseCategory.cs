using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class CourseCategory
    {
        #region "Attributes"
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public String Name { get; set; }
        public String? ImagePath { get; set; }
        #endregion
    }
}
