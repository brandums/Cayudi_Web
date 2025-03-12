using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPlatformWebApp.Model
{
    public class User
    {
        #region "Attributes"
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String? Name { get; set; }
        public String? Description { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        public int? PhoneNumber { get; set; }
        public String? LoginImagePath { get; set; } = string.Empty;
        public String? Profession { get; set; } = string.Empty;

        [PasswordPropertyText]
        public String? Password { get; set; }
        public DateTime CreationDate { get; set; }        
        public int? RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        public bool? IsActivated { get; set; } = false;
        public bool? IsVerified { get; set; } = false;
        public string? ConfirmationToken { get; set; } = string.Empty;
        //[NotMapped]
        //public byte[] ProfImage { get; set; }
        #endregion
    }
}
