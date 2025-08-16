using FlightManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FlightManagement.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        DAPMLThuyetEntities database = new DAPMLThuyetEntities();
        // GET: TrangChu
        public ActionResult Index()
        {
            //var Sach = database.Hotels.Include(c => c.TacGia).Include(c => c.TheLoai).ToList();
            return View(database.Hotels.ToList());
        }
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost] //ghi nhận dữ liệu
        [ValidateAntiForgeryToken]
        public ActionResult Them(Hotel Ks, HttpPostedFileBase HinhAnh)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnh != null && HinhAnh.ContentLength > 0)
                {
                    // Tạo tên file ảnh duy nhất
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);

                    // Lưu ảnh vào thư mục
                    HinhAnh.SaveAs(path);

                    // Lưu đường dẫn vào thuộc tính HinhAnh của mô hình
                    Ks.HinhAnh = "~/Content/Image/" + fileName;
                }
                // Thêm sách vào cơ sở dữ liệu và lưu thay đổi
                database.Hotels.Add(Ks);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(Ks);
        }

        public ActionResult Details(int id)
        {
            return View(database.Hotels.Where(s => s.IdHotel == id).FirstOrDefault());
        }
        [HttpGet]
        public ActionResult Edit(int id)

        {
            Hotel Ks = database.Hotels.FirstOrDefault(p => p.IdHotel == id);
            if (Ks != null)
            {
                return View(Ks);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase HinhAnh, [Bind(Include = "IdHotel,NameHotel,Address,Phone,StarRange,Decripstion,RoomPrice,Location,HinhAnh")] Hotel Ks)
        {
            if (ModelState.IsValid)
            {
                var KSdb = database.Hotels.FirstOrDefault(p => p.IdHotel == Ks.IdHotel);

                if (KSdb != null)
                {
                    // Cập nhật thông tin khách sạn
                    KSdb.NameHotel = Ks.NameHotel;
                    KSdb.Address = Ks.Address;
                    KSdb.Phone = Ks.Phone;
                    KSdb.StarRange = Ks.StarRange;
                    KSdb.Decripstion = Ks.Decripstion;
                    KSdb.Location = Ks.Location;
                    KSdb.RoomPrice = Ks.RoomPrice;

                    // Kiểm tra nếu có ảnh mới được tải lên
                    if (HinhAnh != null && HinhAnh.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(HinhAnh.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);

                        // Tạo thư mục nếu chưa tồn tại
                        var imagePath = Server.MapPath("~/Content/Image");
                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        // Lưu ảnh vào thư mục
                        HinhAnh.SaveAs(path);

                        // Cập nhật đường dẫn hình ảnh
                        KSdb.HinhAnh = "~/Content/Image/" + fileName;
                    }

                    // Lưu tất cả thay đổi vào cơ sở dữ liệu
                    database.SaveChanges();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                    return RedirectToAction("Index");
                }
            }
            return View(Ks);
        }


        public ActionResult Delete(int id)
        {
            return View(database.Hotels.Where(s => s.IdHotel == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Hotel Ks)
        {
            Ks = database.Hotels.Where((s) => s.IdHotel == id).FirstOrDefault();
            database.Hotels.Remove(Ks);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Search()
        {
            // Khởi tạo ViewModel với dữ liệu ban đầu
            var model = new SearchHotel
            {
                NameHotel = null,
                Location = null,
                CheckInDate = null,
                CheckOutDate = null,
                Hotels = new List<Hotel>() // Danh sách rỗng ban đầu
            };
            return View(model); // Hiển thị form tìm kiếm
        }

        [HttpGet]
        public ActionResult SearchHotel(string NameHotel, string Location, DateTime? depatureTime, DateTime? returnTime, decimal? RoomPrice)
        {
            // Lấy danh sách khách sạn từ cơ sở dữ liệu
            var hotelList = database.Hotels.AsQueryable(); // AsQueryable() cho phép linh hoạt với các truy vấn

            // Lọc theo vị trí
            if (!string.IsNullOrEmpty(Location))
            {
                hotelList = hotelList.Where(x => x.Location.Contains(Location));
            }

            // Lọc theo tên khách sạn
            if (!string.IsNullOrEmpty(NameHotel))
            {
                hotelList = hotelList.Where(x => x.NameHotel.Contains(NameHotel));
            }

            // Lọc theo thời gian đặt phòng (Check-in)
            if (depatureTime.HasValue)
            {
                DateTime startOfDay = depatureTime.Value.Date;
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Đảm bảo lọc đúng ngày
                hotelList = hotelList.Where(f => f.depatureTime >= startOfDay && f.depatureTime <= endOfDay);
            }

            // Lọc theo thời gian trả phòng (Check-out)
            if (returnTime.HasValue)
            {
                DateTime startOfDay = returnTime.Value.Date;
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Đảm bảo lọc đúng ngày
                hotelList = hotelList.Where(f => f.returnTime >= startOfDay && f.returnTime <= endOfDay);
            }

            // Lọc theo giá phòng (Room Price)
            if (RoomPrice.HasValue)
            {
                hotelList = hotelList.Where(f => f.RoomPrice <= RoomPrice.Value);
            }

            // Tạo ViewModel với kết quả tìm kiếm
            var model = new SearchHotel
            {
                NameHotel = NameHotel,
                Location = Location,
                CheckInDate = depatureTime,
                CheckOutDate = returnTime,
                Hotels = hotelList.ToList() // Trả về danh sách khách sạn tìm được
            };

            return View(model); // Hiển thị trang kết quả tìm kiếm
        }
        [HttpGet]
        public ActionResult Book(int hotelID)
        {
            // Lấy thông tin khách sạn theo ID
            var hotel = database.Hotels.FirstOrDefault(h => h.IdHotel == hotelID);


            // Tạo model để hiển thị thông tin đặt phòng
            var bookingViewModel = new HotelBooking
            {
                HotelID = hotelID,
                NameHotel = hotel.NameHotel,
                RoomPrice = (decimal)hotel.RoomPrice,
                Location = hotel.Location
            };

            return View(bookingViewModel); // Render ra form đặt phòng
        }

        [HttpPost]
        public ActionResult Book(BookingHotel model)
        {
            if (ModelState.IsValid)
            {
                // Lưu thông tin đặt phòng vào database
                var booking = new BookingHotel
                {
                    IdHotel = model.IdHotel,
                    CheckinDate = model.CheckinDate,
                    CheckoutDate = model.CheckoutDate,
    };
                database.BookingHotels.Add(booking);
                database.SaveChanges();
 
                return RedirectToAction("BookingSuccess"); 
            }

            return View(model); // Hiển thị lại form nếu có lỗi
        }
        public ActionResult BookingSuccess()
        {
            return View();
        }


    }
}