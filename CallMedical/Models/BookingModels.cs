using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    public class Booking
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        [Required]
        public int doctor_id { get; set; }

        [Required]
        public int slot_id { get; set; }

        public string status { get; set; } = "pending";

        public string medical_notes { get; set; }

        public bool share_history_allowed { get; set; } = false;

        public DateTime? created_at { get; set; } = DateTime.Now;

        public virtual PatientProfile Patient { get; set; }

        public virtual DoctorProfile Doctor { get; set; }

        public virtual DoctorSlot Slot { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<SessionNote> SessionNotes { get; set; }
    }

    public class Payment
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int booking_id { get; set; }

        [Required]
        public decimal amount { get; set; }

        [Required]
        public string payment_method { get; set; }

        public string payment_status { get; set; } = "pending";

        public string transaction_id { get; set; }

        public int? cashier_id { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;
        public DateTime? updated_at { get; set; } = DateTime.Now;

        public virtual Booking Booking { get; set; }

        public virtual UsersModel Cashier { get; set; }
    }

    public class SessionNote
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int booking_id { get; set; }

        [Required]
        public int doctor_id { get; set; }

        public string note_content { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;

        public virtual Booking Booking { get; set; }

        public virtual DoctorProfile Doctor { get; set; }
    }
}