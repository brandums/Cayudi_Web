using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPlatformWebApp.Model
{
    public class Cert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String? Source { get; set; }
        public String? Path { get; set; }
        public String Tittle { get; set; }
        public String? CertDescription { get; set; } = "";
        public String? Requeriments { get; set; } = "";

        public String? Description { get; set; } = "application/pdf";
        public int? Order { get; set; }
        public int CourseID { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }
    }
}
