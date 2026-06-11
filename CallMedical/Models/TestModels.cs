using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMedical.Models
{
    [Table("tests")]
    public class Test
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string title { get; set; }

        [Required]
        public string description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }

    [Table("questions")]
    public class Question
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int test_id { get; set; }

        [Required]
        public string question_text { get; set; }

        [Required]
        public int order_index { get; set; }

        [ForeignKey("test_id")]
        public virtual Test Test { get; set; }

        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }
    }

    [Table("question_options")]
    public class QuestionOption
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int question_id { get; set; }

        [Required]
        [MaxLength(255)]
        public string option_text { get; set; }

        [Required]
        public int score_value { get; set; }

        [ForeignKey("question_id")]
        public virtual Question Question { get; set; }
    }

    [Table("test_results")]
    public class TestResult
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int patient_id { get; set; }

        [Required]
        public int test_id { get; set; }

        [Required]
        public int total_score { get; set; }

        [MaxLength(100)]
        public string conclusion { get; set; }

        public DateTime? created_at { get; set; } = DateTime.Now;

        [ForeignKey("patient_id")]
        public virtual PatientProfile Patient { get; set; }

        [ForeignKey("test_id")]
        public virtual Test Test { get; set; }
    }
}