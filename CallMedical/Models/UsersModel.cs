using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    [Table("users")]
    public class UsersModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(150)]
        [Index(IsUnique = true)]
        public string email { get; set; }

        [Required]
        [MaxLength(255)]
        public string password_hash { get; set; }

        [Required]
        [MaxLength(20)]
        public string role { get; set; } // 'admin', 'doctor', 'patient'

        [MaxLength(20)]
        public string status { get; set; } = "active"; // 'pending', 'active', 'suspended'

        public DateTime create_at { get; set; } = DateTime.Now;
        public DateTime updated_at { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
        public virtual ICollection<LoginSession> LoginSessions { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        //public virtual PatientProfile PatientProfile { get; set; } 
        //public virtual DoctorProfile DoctorProfile { get; set; }
        public virtual ICollection<PatientProfile> PatientProfiles { get; set; }
        public virtual ICollection<DoctorProfile> DoctorProfiles { get; set; }
    }

    [Table("password_reset_tokens")]
    public class PasswordResetToken
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [MaxLength(255)]
        public string token { get; set; }

        [Required]
        public DateTime expires_at { get; set; }

        public bool is_used { get; set; } = false;

        public DateTime created_at { get; set; } = DateTime.Now;

        [ForeignKey("user_id")]
        public virtual UsersModel User { get; set; }
    }

    [Table("login_sessions")]
    public class LoginSession
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [MaxLength(45)]
        public string ip_address { get; set; }

        [MaxLength(255)]
        public string device_info { get; set; }

        public DateTime? logged_in_at { get; set; } = DateTime.Now;
        public DateTime? logged_out_at { get; set; }

        [ForeignKey("user_id")]
        public virtual UsersModel User { get; set; }
    }

    [Table("notifications")]
    public class Notification
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [MaxLength(200)]
        public string title { get; set; }

        public string body { get; set; }

        [MaxLength(50)]
        public string type { get; set; }

        public bool is_read { get; set; } = false;

        public DateTime? created_at { get; set; } = DateTime.Now;

        [ForeignKey("user_id")]
        public virtual UsersModel User { get; set; }
    }
}