using EPlatformWebApp.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EPlatformWebApp.DTO
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = String.Empty;    
        public string? Description { get; set; } = String.Empty;
        public int? PhoneNumber { get; set; }
        public string? AvatarPath { get; set; }
        public string Email { get; set; }
        public int? RoleID { get; set; }
        public string ProfImage { get; set; }
        public string Profession { get; set; }
        public string ProfTittleImage { get; set; }
        public bool? IsActivated { get; set; } = false;
        public bool? IsVerified { get; set; } = false;
        public string? ConfirmationToken { get; set; }
    }
}
