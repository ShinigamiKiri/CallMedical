using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    [Table("chat_channels")]
    public class ChatChannel
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        public int? receiver_id { get; set; }

        [MaxLength(20)]
        public string channel_type { get; set; } // 'doctor', 'ai'

        [ForeignKey("patient_id")]
        public virtual PatientProfile Patient { get; set; }

        [ForeignKey("receiver_id")]
        public virtual UsersModel Receiver { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }

    [Table("messages")]
    public class Message
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int channel_id { get; set; }

        [MaxLength(20)]
        public string sender_type { get; set; }

        public string message_text { get; set; }

        [MaxLength(255)]
        public string file_url { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;

        [ForeignKey("channel_id")]
        public virtual ChatChannel ChatChannel { get; set; }
    }

    [Table("ai_red_flags")]
    public class AiRedFlag
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string triggered_keyword { get; set; }

        [Required]
        public string context_message { get; set; }

        [MaxLength(20)]
        public string status { get; set; } = "unresolved";

        public DateTime? created_at { get; set; } = DateTime.Now;

        [ForeignKey("patient_id")]
        public virtual PatientProfile Patient { get; set; }
    }
}