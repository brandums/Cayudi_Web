namespace EPlatformWebApp.DTO
{
    public class UserRegisterDTO
    {
        public String Name { get; set; } = String.Empty;
        public String Email { get; set; }
        public String? Password { get; set; }
    }
}
