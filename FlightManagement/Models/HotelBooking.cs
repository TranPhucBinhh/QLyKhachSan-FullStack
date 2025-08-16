using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FlightManagement.Models
{
    public class HotelBooking
    {
        public int HotelID { get; set; }
        public string NameHotel { get; set; }
        public decimal RoomPrice { get; set; }
        public string Location { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày nhận phòng.")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày trả phòng.")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; internal set; }
    }
}