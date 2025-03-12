namespace EPlatformWebApp.DTO
{
    public class UserCourseDTO
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public bool IsOppened { get; set; }
        public bool IsFinished { get; set; }
        public int Rating { get; set; }
        public int UserID { get; set; }
        public UserDTO User { get; set; }
        public int CourseID { get; set; }
        public CourseDTO Course { get; set; }
    }
}
