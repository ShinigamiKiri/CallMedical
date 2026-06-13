using System.Data.Entity;

namespace CallMedical.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=MentalHealthConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

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
            // --- Users & Auth (users table) ---
            modelBuilder.Entity<UsersModel>().ToTable("users");
            modelBuilder.Entity<UsersModel>().Property(u => u.email).HasMaxLength(150);
            modelBuilder.Entity<UsersModel>().HasIndex(u => u.email).IsUnique();
            modelBuilder.Entity<UsersModel>().Property(u => u.password_hash).HasMaxLength(255);
            modelBuilder.Entity<UsersModel>().Property(u => u.role).HasMaxLength(20);
            modelBuilder.Entity<UsersModel>().Property(u => u.status).HasMaxLength(20);

            // Explicit Foreign Key mappings to fix UsersModel_id bug
            modelBuilder.Entity<PasswordResetToken>().ToTable("password_reset_tokens");
            modelBuilder.Entity<PasswordResetToken>().Property(p => p.token).HasMaxLength(255);
            modelBuilder.Entity<PasswordResetToken>()
                .HasRequired(p => p.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(p => p.user_id);

            modelBuilder.Entity<LoginSession>().ToTable("login_sessions");
            modelBuilder.Entity<LoginSession>().Property(l => l.ip_address).HasMaxLength(45);
            modelBuilder.Entity<LoginSession>().Property(l => l.device_info).HasMaxLength(255);
            modelBuilder.Entity<LoginSession>()
                .HasRequired(l => l.User)
                .WithMany(u => u.LoginSessions)
                .HasForeignKey(l => l.user_id);

            modelBuilder.Entity<Notification>().ToTable("notifications");
            modelBuilder.Entity<Notification>().Property(n => n.title).HasMaxLength(200);
            modelBuilder.Entity<Notification>().Property(n => n.type).HasMaxLength(50);
            modelBuilder.Entity<Notification>()
                .HasRequired(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.user_id);

            // --- Profiles ---
            modelBuilder.Entity<PatientProfile>().ToTable("patients_profile");
            modelBuilder.Entity<PatientProfile>().Property(p => p.full_name).HasMaxLength(100);
            modelBuilder.Entity<PatientProfile>().Property(p => p.phone).HasMaxLength(15);
            modelBuilder.Entity<PatientProfile>().Property(p => p.gender).HasMaxLength(20);
            modelBuilder.Entity<PatientProfile>().Property(p => p.dob).HasColumnType("date");
            modelBuilder.Entity<PatientProfile>()
                .HasRequired(p => p.User)
                .WithMany(u => u.PatientProfiles)
                .HasForeignKey(p => p.user_id);

            modelBuilder.Entity<DoctorProfile>().ToTable("doctor_profiles");
            modelBuilder.Entity<DoctorProfile>().Property(d => d.full_name).HasMaxLength(100);
            modelBuilder.Entity<DoctorProfile>().Property(d => d.specialty).HasMaxLength(150);
            modelBuilder.Entity<DoctorProfile>().Property(d => d.certificate_url).HasMaxLength(255);
            modelBuilder.Entity<DoctorProfile>()
                .HasRequired(d => d.User)
                .WithMany(u => u.DoctorProfiles)
                .HasForeignKey(d => d.user_id);

            modelBuilder.Entity<DoctorSlot>().ToTable("doctor_slots");
            modelBuilder.Entity<DoctorSlot>()
                .HasRequired(s => s.Doctor)
                .WithMany(d => d.DoctorSlots)
                .HasForeignKey(s => s.doctor_id);

            // --- Tests ---
            modelBuilder.Entity<Test>().ToTable("tests");
            modelBuilder.Entity<Test>().Property(t => t.title).HasMaxLength(100);

            modelBuilder.Entity<Question>().ToTable("questions");
            modelBuilder.Entity<Question>()
                .HasRequired(q => q.Test)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.test_id);

            modelBuilder.Entity<QuestionOption>().ToTable("question_options");
            modelBuilder.Entity<QuestionOption>().Property(o => o.option_text).HasMaxLength(255);
            modelBuilder.Entity<QuestionOption>()
                .HasRequired(o => o.Question)
                .WithMany(q => q.QuestionOptions)
                .HasForeignKey(o => o.question_id);

            modelBuilder.Entity<TestResult>().ToTable("test_results");
            modelBuilder.Entity<TestResult>().Property(r => r.conclusion).HasMaxLength(100);
            modelBuilder.Entity<TestResult>()
                .HasRequired(r => r.Patient)
                .WithMany(p => p.TestResults)
                .HasForeignKey(r => r.patient_id);
            modelBuilder.Entity<TestResult>()
                .HasRequired(r => r.Test)
                .WithMany(t => t.TestResults)
                .HasForeignKey(r => r.test_id);

            // --- Bookings & Payments ---
            modelBuilder.Entity<Booking>().ToTable("bookings");
            modelBuilder.Entity<Booking>().Property(b => b.status).HasMaxLength(20);
            modelBuilder.Entity<Booking>()
                .HasRequired(b => b.Patient)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.patient_id);
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

            modelBuilder.Entity<Payment>().ToTable("payments");
            modelBuilder.Entity<Payment>().Property(p => p.payment_method).HasMaxLength(20);
            modelBuilder.Entity<Payment>().Property(p => p.payment_status).HasMaxLength(20);
            modelBuilder.Entity<Payment>().Property(p => p.transaction_id).HasMaxLength(100);
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.booking_id);
            modelBuilder.Entity<Payment>()
                .HasOptional(p => p.Cashier)
                .WithMany()
                .HasForeignKey(p => p.cashier_id);

            modelBuilder.Entity<SessionNote>().ToTable("session_notes");
            modelBuilder.Entity<SessionNote>()
                .HasRequired(n => n.Booking)
                .WithMany(b => b.SessionNotes)
                .HasForeignKey(n => n.booking_id);
            modelBuilder.Entity<SessionNote>()
                .HasRequired(n => n.Doctor)
                .WithMany()
                .HasForeignKey(n => n.doctor_id);

            // --- Chat ---
            modelBuilder.Entity<ChatChannel>().ToTable("chat_channels");
            modelBuilder.Entity<ChatChannel>().Property(c => c.channel_type).HasMaxLength(20);
            modelBuilder.Entity<ChatChannel>()
                .HasRequired(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.patient_id);
            modelBuilder.Entity<ChatChannel>()
                .HasOptional(c => c.Receiver)
                .WithMany()
                .HasForeignKey(c => c.receiver_id);

            modelBuilder.Entity<Message>().ToTable("messages");
            modelBuilder.Entity<Message>().Property(m => m.sender_type).HasMaxLength(20);
            modelBuilder.Entity<Message>().Property(m => m.file_url).HasMaxLength(255);
            modelBuilder.Entity<Message>()
                .HasRequired(m => m.ChatChannel)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.channel_id);

            modelBuilder.Entity<AiRedFlag>().ToTable("ai_red_flags");
            modelBuilder.Entity<AiRedFlag>().Property(f => f.triggered_keyword).HasMaxLength(100);
            modelBuilder.Entity<AiRedFlag>().Property(f => f.status).HasMaxLength(20);
            modelBuilder.Entity<AiRedFlag>()
                .HasRequired(f => f.Patient)
                .WithMany()
                .HasForeignKey(f => f.patient_id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
