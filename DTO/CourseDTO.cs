namespace EPlatformWebApp.DTO
{
    public class CourseDTO
    {
        public int ID { get; set; }
        public string Tittle { get; set; }
        public string Subtitle { get; set; }
        public int CourseCategoryID { get; set; }
        public string Description { get; set; }
        public string Content { get; set; } 
        public float Price { get; set; }
        public int TrainerID { get; set; }
        public DateTime StartDate { get; set; }
        public string ImagePath { get; set; } 
        public string BannerPath { get; set; }
        public int DurationTime { get; set; }
        public CourseCategoryDTO CourseCategory { get; set; }

    }
}
