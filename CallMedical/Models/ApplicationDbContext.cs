using System.Data.Entity;

namespace CallMedical.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Tên "MentalHealthConnection" phải khớp chính xác với name trong thẻ <connectionStrings> của file Web.config
        public ApplicationDbContext() : base("name=MentalHealthConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        // Khai báo toàn bộ DbSet cho hệ thống
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<LoginSession> LoginSessions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }
        public DbSet<DoctorSlot> DoctorSlots { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SessionNote> SessionNotes { get; set; }

        public DbSet<ChatChannel> ChatChannels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<AiRedFlag> AiRedFlags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Cấu hình ngăn việc xóa dây chuyền (Cascade Delete) gây xung đột vòng lặp ở bảng Bookings
            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Doctor)
                .WithMany(d => d.Bookings)
                .HasForeignKey(b => b.doctor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Slot)
                .WithMany()
                .HasForeignKey(b => b.slot_id)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}