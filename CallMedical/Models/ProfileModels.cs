using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    public class PatientProfile
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public string full_name { get; set; }

        public string phone { get; set; }

        public DateTime? dob { get; set; }

        public string gender { get; set; }

        public virtual UsersModel User { get; set; }

        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }

    public class DoctorProfile
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int user_id { get; set; }

        [Required]
        public string full_name { get; set; }

        [Required]
        public string specialty { get; set; }

        [Required]
        public string biography { get; set; }

        public decimal? price_per_session { get; set; }

        public string certificate_url { get; set; }

        public bool is_verified { get; set; } = false;

        public virtual UsersModel User { get; set; }

        public virtual ICollection<DoctorSlot> DoctorSlots { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }

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

        public virtual DoctorProfile Doctor { get; set; }
    }
}