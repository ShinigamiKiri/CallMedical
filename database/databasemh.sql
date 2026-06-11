USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'MentalHealthDB')
BEGIN
    ALTER DATABASE MentalHealthDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MentalHealthDB;
END
GO

CREATE DATABASE MentalHealthDB;
GO

USE MentalHealthDB;
GO

--Tai khoan 
CREATE TABLE users (
	id INT IDENTITY PRIMARY KEY,
	email VARCHAR(150) UNIQUE NOT NULL,
	password_hash VARCHAR(255) NOT NULL,
	[role] VARCHAR(20) NOT NULL CHECK ([role] IN ('admin', 'doctor', 'patient')),
	status VARCHAR(20) DEFAULT 'active' CHECK (status IN ('pending', 'active', 'suspended')),
	create_at DATETIME DEFAULT GETDATE(),
	updated_at DATETIME DEFAULT GETDATE()
);
GO

--Quen mat khau
CREATE TABLE password_reset_tokens (
    id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    token VARCHAR(255) NOT NULL,
    expires_at DATETIME NOT NULL,
    is_used BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_reset_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
GO

--Bao mat tai khoan
CREATE TABLE login_sessions (
    id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    ip_address VARCHAR(45),
    device_info NVARCHAR(255),
    logged_in_at DATETIME DEFAULT GETDATE(),
    logged_out_at DATETIME NULL,
    CONSTRAINT FK_session_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
GO

--noti
CREATE TABLE notifications (
    id INT IDENTITY PRIMARY KEY,
    user_id INT NOT NULL,
    title NVARCHAR(200) NOT NULL,
    body NVARCHAR(MAX),
    type VARCHAR(50) CHECK (type IN ('booking', 'payment', 'test_result', 'system', 'chat')),
    is_read BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_notif_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
GO

--Thong tin benh nhan
CREATE TABLE patients_profile (
	id INT IDENTITY PRIMARY KEY,
	[user_id] INT NOT NULL,
	full_name NVARCHAR(100) NOT NULL,
	phone VARCHAR(15),
	dob DATE,
	gender NVARCHAR(20) CHECK (gender IN (N'Nam', N'Nữ', N'Khác')),
	CONSTRAINT FK_patient_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
GO

--Thong tin bac si
CREATE TABLE doctor_profiles (
	id INT IDENTITY PRIMARY KEY,
	[user_id] INT NOT NULL,
	full_name NVARCHAR(100) NOT NULL,
	specialty NVARCHAR(150) NOT NULL,
	biography NVARCHAR(MAX) NOT NULL,
	price_per_session DECIMAL (18, 2),
	[certificate_url] VARCHAR(255),
	is_verified BIT DEFAULT 0,
	CONSTRAINT FK_doctor_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
GO

--Quan li lich doctor 
CREATE TABLE doctor_slots (
	id INT IDENTITY PRIMARY KEY,
	doctor_id INT NOT NULL,
	start_time DATETIME NOT NULL,
	end_time DATETIME NOT NULL,
	is_available BIT DEFAULT 1,
	CONSTRAINT FK_slots_doctor FOREIGN KEY (doctor_id) REFERENCES doctor_profiles(id) ON DELETE CASCADE
);
GO

--Quan li bai test
CREATE TABLE tests (
	id INT IDENTITY PRIMARY KEY,
	title NVARCHAR(100) NOT NULL,
	[description] NVARCHAR(MAX) NOT NULL
);
GO

--Quan li cau hoi
CREATE TABLE questions (
	id INT IDENTITY PRIMARY KEY,
	test_id INT NOT NULL,
	question_text NVARCHAR(MAX) NOT NULL,
	order_index INT NOT NULL,
	CONSTRAINT FK_questions_test FOREIGN KEY (test_id) REFERENCES tests(id) ON DELETE CASCADE
);
GO

--Quan li dap an
CREATE TABLE question_options (
    id INT IDENTITY(1,1) PRIMARY KEY,
    question_id INT NOT NULL,
    option_text NVARCHAR(255) NOT NULL,
    score_value INT NOT NULL,
    CONSTRAINT FK_options_question FOREIGN KEY (question_id) REFERENCES questions(id) ON DELETE CASCADE
);
GO

--Quan li lich su lam bai
CREATE TABLE test_results (
    id INT IDENTITY(1,1) PRIMARY KEY,
    patient_id INT NOT NULL,
    test_id INT NOT NULL,
    total_score INT NOT NULL,
    conclusion NVARCHAR(100),
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_results_patient FOREIGN KEY (patient_id) REFERENCES patients_profile(id) ON DELETE CASCADE,
    CONSTRAINT FK_results_test FOREIGN KEY (test_id) REFERENCES tests(id) ON DELETE NO ACTION
);
GO

--Quan li lich hen
CREATE TABLE bookings (
    id INT IDENTITY(1,1) PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    slot_id INT NOT NULL,
    status VARCHAR(20) DEFAULT 'pending' CHECK (status IN ('pending', 'confirmed', 'cancelled', 'completed')),
    medical_notes NVARCHAR(MAX),
    share_history_allowed BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_bookings_patient FOREIGN KEY (patient_id) REFERENCES patients_profile(id) ON DELETE CASCADE,
    CONSTRAINT FK_bookings_doctor FOREIGN KEY (doctor_id) REFERENCES doctor_profiles(id) ON DELETE NO ACTION,
    CONSTRAINT FK_bookings_slot FOREIGN KEY (slot_id) REFERENCES doctor_slots(id) ON DELETE NO ACTION
);
GO

-- Quan li thanh toan va doanh thu
CREATE TABLE payments (
    id INT IDENTITY(1,1) PRIMARY KEY,
    booking_id INT NOT NULL,
    amount DECIMAL(18, 2) NOT NULL, 
    payment_method VARCHAR(20) NOT NULL CHECK (payment_method IN ('cash', 'banking', 'e-wallet')), 
    payment_status VARCHAR(20) DEFAULT 'pending' CHECK (payment_status IN ('pending', 'paid', 'refunded')), 
    transaction_id VARCHAR(100) NULL, 
    cashier_id INT NULL, 
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_payments_booking FOREIGN KEY (booking_id) REFERENCES bookings(id) ON DELETE CASCADE,
    CONSTRAINT FK_payments_cashier FOREIGN KEY (cashier_id) REFERENCES users(id) ON DELETE NO ACTION
);
GO

-- Ho so benh nhan va ca kham
CREATE TABLE session_notes (
    id INT IDENTITY PRIMARY KEY,
    booking_id INT NOT NULL,
    doctor_id INT NOT NULL,
    note_content NVARCHAR(MAX),
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_note_booking FOREIGN KEY (booking_id) REFERENCES bookings(id) ON DELETE CASCADE,
    CONSTRAINT FK_note_doctor FOREIGN KEY (doctor_id) REFERENCES doctor_profiles(id) ON DELETE NO ACTION
);
GO

--Quan li chat
CREATE TABLE chat_channels (
    id INT IDENTITY(1,1) PRIMARY KEY,
    patient_id INT NOT NULL,
    receiver_id INT NULL, 
    channel_type VARCHAR(20) CHECK (channel_type IN ('doctor', 'ai')),
    CONSTRAINT FK_channels_patient FOREIGN KEY (patient_id) REFERENCES patients_profile(id) ON DELETE CASCADE,
    CONSTRAINT FK_channels_receiver FOREIGN KEY (receiver_id) REFERENCES users(id) ON DELETE NO ACTION
);
GO

--Quan li chi tiet tin nhan
CREATE TABLE messages (
    id INT IDENTITY(1,1) PRIMARY KEY,
    channel_id INT NOT NULL,
    sender_type VARCHAR(20) CHECK (sender_type IN ('patient', 'doctor', 'ai')),
    message_text NVARCHAR(MAX),
    file_url VARCHAR(255) NULL,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_messages_channel FOREIGN KEY (channel_id) REFERENCES chat_channels(id) ON DELETE CASCADE
);
GO

--Quan li giam sat an toan
CREATE TABLE ai_red_flags (
    id INT IDENTITY(1,1) PRIMARY KEY,
    patient_id INT NOT NULL,
    triggered_keyword NVARCHAR(100) NOT NULL,
    context_message NVARCHAR(MAX) NOT NULL,
    status VARCHAR(20) DEFAULT 'unresolved' CHECK (status IN ('unresolved', 'resolved')),
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_flags_patient FOREIGN KEY (patient_id) REFERENCES patients_profile(id) ON DELETE CASCADE
);
GO

--du lieu mau