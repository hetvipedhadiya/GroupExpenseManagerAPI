using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.Model
{
    public class UserModel
    {
        [Key]
        public int HostId { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string? Password { get; set; }

        [StringLength(200)]
        public string? ConfirmPassword { get; set; }

        [StringLength(15)]
        public string? MobileNo { get; set; }

        public DateTime? CreateAt { get; set; }

        public int? IsActive { get; set; }
    }

    public class LogInModel
    {
        public string Email { get; set; }

        [StringLength(200)]
        public string? Password { get; set; }
    }

    public class RegistrationModel
    {
        public string Email { get; set; }

        [StringLength(200)]
        public string? Password { get; set; }
        [StringLength(200)]
        public string? ConfirmPassword { get; set; }

        [StringLength(15)]
        public string? MobileNo { get; set; }

    }
}
