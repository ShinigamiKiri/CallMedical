1. Hướng dẫn thiết lập (Dành cho GitHub/Người cộng tác)
📋 Prerequisites (Yêu cầu)
Visual Studio: 2019 hoặc 2022.

.NET Framework: 4.8.

SQL Server: Mở SSMS, chạy file database/databasemh.sql để tạo Database MentalHealthDB.

🚀 Cài đặt dự án
Clone dự án: Tải hoặc clone code về máy.

Restore NuGet Packages:

Chuột phải vào Solution 'CallMedical' trong Solution Explorer.

Chọn Restore NuGet Packages.

Nếu vẫn lỗi, mở Package Manager Console và chạy lệnh: Update-Package -Reinstall.

Cấu hình Database:

Mở file Web.config.

Tại mục <connectionStrings>, sửa chuỗi kết nối khớp với Server SQL của bạn:

XML
<add name="MentalHealthConnection" 
     connectionString="Data Source=TEN_SERVER_CUA_BAN;Initial Catalog=MentalHealthDB;Integrated Security=True;" 
     providerName="System.Data.SqlClient" />
Build: Nhấn Ctrl + Shift + B. Dự án đã được cấu hình Fluent API, không dùng DataAnnotations nên sẽ không bị lỗi đụng hàng thư viện.

2. Technical Troubleshooting Log (Dành cho AI tiếp quản)
Đây là tóm tắt toàn bộ lịch sử lỗi và các thay đổi kiến trúc để AI tiếp theo không đi vào "vết xe đổ":

📌 Tóm tắt bối cảnh
Project: ASP.NET MVC, .NET Framework 4.8.

Database: MS SQL Server (MentalHealthDB).

Thư viện: Entity Framework 6.4.4, BCrypt.Net-Next 3.3.0.

⚠️ Các vấn đề đã xử lý
Lỗi CS0433 (Ambiguous Reference):

Nguyên nhân: Xung đột giữa System.ComponentModel.DataAnnotations (System) và EntityFramework khi sử dụng các Annotation ([Table], [Key], v.v.).

Giải pháp: Đã gỡ bỏ toàn bộ DataAnnotations khỏi các file Model. Chuyển sang sử dụng Fluent API trong ApplicationDbContext.cs (OnModelCreating) để ánh xạ bảng và quan hệ. Đây là giải pháp triệt để.

Lỗi 'System.Runtime 10.0.0.0' not found:

Nguyên nhân: Do cài đặt nhầm phiên bản BCrypt.Net-Next mới nhất (dành cho .NET 8/10+) vào project .NET Framework 4.8.

Giải pháp: Gỡ bỏ bản mới, cài đặt lại BCrypt.Net-Next bản 3.3.0 (phiên bản ổn định cho .NET Framework).

Lỗi 404/500 (Route & Column Mismatch):

Route: Frontend fetch sai URL (/api/auth/register), đã chuyển về /Account/Register.

Mapping: Lỗi Invalid column name 'UsersModel_id' xảy ra do EF tự động suy diễn Foreign Key (Convention over Configuration). Đã sửa bằng cách mapping tường minh trong Fluent API:

C#
modelBuilder.Entity<[EntityName]>()
    .HasRequired(x => x.[NavigationProperty])
    .WithMany()
    .HasForeignKey(x => x.user_id);