using FlightManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static FlightManagement.Models.SearchHotel;
using static FlightManagement.Models.SearchFlight;

namespace FlightManagement.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly DAPMLThuyetEntities _context;
        DAPMLThuyetEntities database = new DAPMLThuyetEntities();
        // GET: UserHome
        public ActionResult Index()
        {
            return View();
        }
    }

}