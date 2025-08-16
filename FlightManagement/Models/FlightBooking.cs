using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace FlightManagement.Models
{
    public class FlightBooking
    {
        // Thông tin chuyến bay
        public int FlightID { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal FlightPrice { get; set; }

        // Thông tin người dùng
        public CustomerInfo CustomerInfo { get; set; }
    }


}