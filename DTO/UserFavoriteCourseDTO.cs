namespace EPlatformWebApp.DTO
{
    public class UserFavoriteCourseDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public UserDTO User { get; set; }
        public int CourseID { get; set; }
        public CourseDTO Course { get; set; }
    }
}
