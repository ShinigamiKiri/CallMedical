using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    public class ChatChannel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        public int? receiver_id { get; set; }

        public string channel_type { get; set; } // 'doctor', 'ai'

        public virtual PatientProfile Patient { get; set; }

        public virtual UsersModel Receiver { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }

    public class Message
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int channel_id { get; set; }

        public string sender_type { get; set; }

        public string message_text { get; set; }

        public string file_url { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;

        public virtual ChatChannel ChatChannel { get; set; }
    }

    public class AiRedFlag
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        [Required]
        public string triggered_keyword { get; set; }

        [Required]
        public string context_message { get; set; }

        public string status { get; set; } = "unresolved";

        public DateTime? created_at { get; set; } = DateTime.Now;

        public virtual PatientProfile Patient { get; set; }
    }
}