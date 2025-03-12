using EPlatformWebApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.DTO
{
    public class CourseTestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int CourseID { get; set; }
        public CourseDTO Course { get; set; }
    }
}
