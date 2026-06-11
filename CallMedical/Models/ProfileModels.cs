using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    [Table("patients_profile")]
    public class PatientProfile
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string full_name { get; set; }

        [MaxLength(15)]
        public string phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dob { get; set; }

        [MaxLength(20)]
        public string gender { get; set; }

        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }

    [Table("doctor_profiles")]
    public class DoctorProfile
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string full_name { get; set; }

        [Required]
        [MaxLength(150)]
        public string specialty { get; set; }

        [Required]
        public string biography { get; set; }

        public decimal? price_per_session { get; set; }

        [MaxLength(255)]
        public string certificate_url { get; set; }

        public bool is_verified { get; set; } = false;

        [ForeignKey("user_id")]
        public virtual UsersModel User { get; set; }

        public virtual ICollection<DoctorSlot> DoctorSlots { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }

    [Table("doctor_slots")]
    public class DoctorSlot
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int doctor_id { get; set; }

        [Required]
        public DateTime start_time { get; set; }

        [Required]
        public DateTime end_time { get; set; }

        public bool is_available { get; set; } = true;

        [ForeignKey("doctor_id")]
        public virtual DoctorProfile Doctor { get; set; }
    }
}