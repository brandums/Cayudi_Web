namespace EPlatformWebApp.DTO
{
    public class UserCourseCreationDTO
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public bool IsOnline { get; set; }
        public bool IsOppened { get; set; }
    }
}
