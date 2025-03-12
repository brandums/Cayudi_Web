using EPlatformWebApp.DTO;


namespace EPlatformWebApp.DTO
{
    public class VideoDTO
    {
        public String? Source { get; set; }
        public String? Path { get; set; }
        public String Tittle { get; set; }
        public String? Thumbnail { get; set; }
        public String? Description { get; set; }
        public int? Order { get; set; }
        public int CourseID { get; set; }    
    }
}
