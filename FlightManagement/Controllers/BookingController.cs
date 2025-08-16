using FlightManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FlightManagement.Controllers
{
    public class BookingController : Controller
    {
        DAPMLThuyetEntities database = new DAPMLThuyetEntities();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookAD = database.Bookings.ToList();
            return View(bookAD);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(database.Accounts, "accountID", "firstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "bookingID,bookingDate,totalAmount,status,accountID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.bookingID = 0;
                database.Bookings.Add(booking);
                database.SaveChanges();
                TempData["SuccessMessage"] = "Tạo thành công!";
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(database.Accounts, "accountID", "firstName", booking.accountID);
            return View(booking);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Booking booking = database.Bookings.FirstOrDefault(p => p.bookingID == id);
            if (booking != null)
            {
                ViewBag.AccountID = new SelectList(database.Accounts, "accountID", "firstName", booking.accountID);
                return View(booking);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "bookingID,bookingDate,totalAmount,status,accountID")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var bookingDB = database.Bookings.FirstOrDefault(p => p.bookingID == booking.bookingID);
                if (bookingDB != null)
                {
                    bookingDB.bookingDate = booking.bookingDate;
                    bookingDB.totalAmount = booking.totalAmount;
                    bookingDB.status = booking.status;
                    bookingDB.accountID = booking.accountID;

                    database.SaveChanges();
                    TempData["SuccessMessage"] = "Chỉnh sửa thành công!";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AccountID = new SelectList(database.Accounts, "accountID", "firstName", booking.accountID);
            return View(booking);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Booking booking = database.Bookings.FirstOrDefault(p => p.bookingID == id);
            if (booking != null)
            {
                return View(booking);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var bookingDB = database.Bookings.FirstOrDefault(p => p.bookingID == id);
            if (bookingDB != null)
            {
                database.Bookings.Remove(bookingDB);
                database.SaveChanges();
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Book(int flightID)
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Login"); // Chuyển hướng đến trang login nếu chưa đăng nhập
            }
            // Lấy thông tin chuyến bay từ flightID
            var flight = database.Flights.Find(flightID);

            if (flight == null)
            {
                return HttpNotFound();
            }

            // Tạo ViewModel để truyền dữ liệu chuyến bay và thông tin người dùng
            var model = new FlightBooking
            {
                FlightID = flight.flightID,
                DepartureCity = flight.departureCity,
                ArrivalCity = flight.arrivalCity,
                DepartureTime = flight.departureTime,
                FlightPrice = flight.flightPrice,
                CustomerInfo = new CustomerInfo() // Khởi tạo để người dùng có thể nhập thông tin
            };

            return View(model);
        }

        // Xử lý khi người dùng gửi thông tin đặt vé
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(FlightBooking model)
        {
            if (ModelState.IsValid)
            {
                // Tạo Booking mới
                var booking = new Booking
                {
                    bookingDate = DateTime.Now,
                    totalAmount = model.FlightPrice, // Giá vé
                    status = "Đang chờ thanh toán",
                    flightID = model.FlightID,
                    accountID = GetLoggedInUserAccountID() // Lấy accountID từ người dùng đã đăng nhập
                };

                // Thêm vào cơ sở dữ liệu
                database.Bookings.Add(booking);
                database.SaveChanges();

                // Tạo CustomerInfo từ thông tin người dùng
                var customerInfo = new CustomerInfo
                {
                    bookingID = booking.bookingID,
                    FullName = model.CustomerInfo.FullName,
                    Email = model.CustomerInfo.Email,
                    PhoneNumber = model.CustomerInfo.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    flightID = model.FlightID,
                };

                // Thêm CustomerInfo vào cơ sở dữ liệu
                database.CustomerInfoes.Add(customerInfo);
                database.SaveChanges();

                // Thông báo thành công
                TempData["SuccessMessage"] = "Đặt vé thành công!";
                return RedirectToAction("Index", "Userhome"); // Chuyển hướng đến danh sách các booking
            }

            // Nếu có lỗi, hiển thị lại form
            return View(model);
        }

        // Lấy accountID của người dùng đã đăng nhập (giả sử bạn có cơ chế để lấy accountID)
        private int? GetLoggedInUserAccountID()
        {
            // Giả sử bạn có cách lấy thông tin accountID từ session hoặc authentication
            return 1; // Thay thế bằng logic lấy thông tin từ session hoặc token của người dùng đã đăng nhập
        }
    }
}
