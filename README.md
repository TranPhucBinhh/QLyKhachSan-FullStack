# QLyKhachSan-FullStack

> Dự án học tập: Website **Quản lý Khách sạn** (quy mô môn học/đồ án).  
> Mục tiêu: quản lý phòng – đặt phòng – khách hàng – dịch vụ – hóa đơn – báo cáo.

> **Lưu ý**: Trong repo hiện tại có solution tên **`FlightManagement.sln`** và thư mục `FlightManagement` (sẽ đổi tên về *HotelManagement* trong các bản sau).

---

## 🧰 Công nghệ (dự kiến trong repo này)
- **.NET/Visual Studio Solution** (MVC / WebApp)  
- **C#** cho backend; **HTML/CSS/JavaScript** cho giao diện
- **SQL Server** (Entity Framework/Migrations hoặc script `.sql`).

### 1) Quản lý người dùng & phân quyền
- [✅] Đăng nhập/Đăng xuất (Session/Cookie)
- [✅ ] Phân quyền: Admin / Người dùng
- [✅] Đổi mật khẩu, cập nhật thông tin cá nhân

### 2) Quản lý phòng
- [✅] CRUD **Phòng**
- [✅ ] **Loại phòng** (tên loại, số người tối đa, giá theo đêm)
- [✅ ] **Tình trạng phòng** (Trống/Đang ở/Đang dọn/Đang bảo trì)
- [✅ ] Ảnh phòng, mô tả, tiện nghi

### 3) Đặt phòng & lịch ở
- [✅ ] Tạo đặt phòng (check-in/check-out, số đêm, số khách)
- [✅ ] Giữ phòng / hủy phòng
- [ ] Xếp phòng khi check-in
- [ ] Gia hạn thời gian ở, đổi phòng

### 4) Khách hàng
- [✅ ] CRUD **Khách hàng**
- [ ] Tra cứu theo số điện thoại/CMND/Passport
- [ ] Lịch sử đặt phòng của khách

### 5) Dịch vụ đi kèm
- [ ] Danh mục dịch vụ (ăn uống, giặt ủi, đưa đón…)
- [ ] Ghi nhận **sử dụng dịch vụ** theo phòng/phiếu lưu trú
- [✅ ] Tính tiền dịch vụ theo số lượng/đơn giá

### 6) Hóa đơn & thanh toán
- [✅] Tạo hóa đơn khi **checkout**
- [ ] Tính tiền phòng (theo số đêm, phụ thu cuối tuần/lễ nếu có)
- [ ] Tính tiền dịch vụ, giảm giá/khuyến mãi
- [ ] Trạng thái thanh toán (Chưa trả/Đã trả/Công nợ)

### 7) Báo cáo & thống kê
- [ ] Doanh thu theo **ngày/tuần/tháng**
- [✅ ] Top dịch vụ sử dụng nhiều nhất
- [✅ ] Xuất **Excel/PDF** (nếu có)

---

## 🗂 Cấu trúc thư mục
```
QLyKhachSan-FullStack/
├─ FlightManagement.sln              
└─ FlightManagement/                 
   ├─ Controllers/
   ├─ Models/
   ├─ Views/
   └─ ... 
```
> Tùy cấu trúc thực tế trong repo, cập nhật lại cho chính xác.

---

## 🚀 Hướng dẫn chạy dự án (local)

### Cách 1: Visual Studio
1. Cài **Visual Studio 2022** (có workload **ASP.NET và phát triển web**).
2. Mở `FlightManagement.sln`.
3. Kiểm tra `appsettings.json` (hoặc `Web.config`) và **cập nhật chuỗi kết nối** đến SQL Server local.
4. Khởi tạo CSDL:
   - **Nếu dùng EF Migrations**: chạy `Update-Database` (Package Manager Console) hoặc `dotnet ef database update`.
   - **Nếu có script .sql**: chạy file `.sql` trong SQL Server trước.
5. Chọn dự án Web làm **Startup Project** → **F5** để chạy.

## 🧪 Dữ liệu mẫu
- Tài khoản mặc định: `admin` / `123456` *


## 🛣 Lộ trình phát triển tiếp theo (gợi ý)
- Đổi tên solution/project sang **HotelManagement** để thống nhất.
- Bổ sung unit test cho tính tiền phòng & dịch vụ.
- Thêm Docker Compose (SQL Server + WebApp).
- Tối ưu UI/UX, responsive, dark mode.
- CI/CD GitHub Actions (build & run test).


## 👤 Tác giả
- TranPhucBinhh 
- Liên hệ: tranphucbinh201@gmail.com

