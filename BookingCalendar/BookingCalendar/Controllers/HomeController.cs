using System.Linq;
using System.Web.Mvc;

namespace BookingCalendar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageAvailability()
        {
            return View();
        }

        public JsonResult GetBookings()
        {
            using (BookingDatabaseEntities dc = new BookingDatabaseEntities())
            {
                var events = dc.Bookings.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Booking e)
        {
            var status = false;
            using (BookingDatabaseEntities dc = new BookingDatabaseEntities())
            {
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Bookings.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Start = e.Start;
                        v.Price = e.Price;
                        v.End = e.End;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    e.IsAvailable = true;
                    dc.Bookings.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }

        [Authorize]
        [HttpPost]
        public JsonResult BookEvent(int EventID)
        {
            var status = false;
            using (BookingDatabaseEntities dc = new BookingDatabaseEntities())
            {
                if (EventID > 0)
                {
                    var username = User.Identity.Name;
                    var v = dc.Bookings.Where(a => a.EventID == EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.BookedBy = username;
                        v.IsAvailable = false;
                        v.ThemeColor = "red";
                        dc.SaveChanges();
                        status = true;
                    }
                }

            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (BookingDatabaseEntities dc = new BookingDatabaseEntities())
            {
                var v = dc.Bookings.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Bookings.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}