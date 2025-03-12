namespace EPlatformWebApp.DTO
{
    public class FileDataDTO
    {
        public string SourceID { get; set; }
        public string SourceType { get; set; }
        public string CourseSource { get; set; }
        public string imageType { get; set; }
        public IFormFile File { get; set; }
    }
}
