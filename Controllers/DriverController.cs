using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspDotNetSummerProject.Models;
using AspDotNetSummerProject.Models.Db;
using Newtonsoft.Json;

namespace AspDotNetSummerProject.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        public ActionResult Home()
        {
            return View();
        }
        //creat an driver account
        [HttpGet]
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(user u, driver d)
        {
            if (ModelState.IsValid)
            {
                var db = new AspdotNetSummerDBEntities();
                u.user_type = "driver";
                db.users.Add(u);
                db.SaveChanges();
                var user = (from p in db.users where u.email.Equals(p.email) select p).SingleOrDefault();
                Session["logged_user"] = user.user_id;
                d.user_id = user.user_id;
                ViewBag.driver_name=d.driver_name ;
                ViewBag.license_no = d.license_no;
                ViewBag.driver_nid = d.driver_nid;
                ViewBag.vechile_no = d.vechile_no;
                ViewBag.vechile_type = d.vechile_type;
                ViewBag.dob = d.dob;
                ViewBag.address=d.address;
                ViewBag.phone =d.phone;
                d.manger_id = 1;
                db.drivers.Add(d);
                db.SaveChanges();
                return RedirectToAction("profile", "Driver");
            }
            else
            {
                return View(u);
            }
            
        }

        public ActionResult profile()
        {
            var tempuser_id = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.drivers where p.user_id == tempuser_id select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempuser_id select n).SingleOrDefault();
            ViewBag.email = usersinfo.email;
            ViewBag.pasword = usersinfo.password;

            return View(user);
        }
        //logout
        public ActionResult Logout()
        {
            Session.Remove("logged_user");
            return RedirectToAction("Login", "Home");
        }
        //edit profile
        [HttpGet]
        public ActionResult edit(user u, driver d)
        {
            var tempuser_id = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.drivers where p.user_id == tempuser_id select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempuser_id select n).SingleOrDefault();
            ViewBag.email = usersinfo.email;
            ViewBag.pasword = usersinfo.password;

            return View(user);
        }
        [HttpPost]
        public ActionResult edit(trip_operations t, user u, driver d)
        {
                 AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
                 var tempuser_id = ((int)Session["logged_user"]);
                 var st = (from s in db.users where s.user_id == tempuser_id select s).SingleOrDefault();
                 var stt = (from s in db.drivers where s.user_id == tempuser_id select s).SingleOrDefault();
                 stt.driver_name = d.driver_name;           
                 stt.vechile_no= d.vechile_no;
                 stt.license_no = d.license_no;
                 stt.vechile_type= d.vechile_type;
                 stt.phone = d.phone;
                 stt.driver_nid = d.driver_nid;
                 stt.dob = d.dob;
                 stt.address = d.address;
                 db.SaveChanges();
                 return RedirectToAction("profile", "Driver");

        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(user u, TempClass t)
        {
            if (ModelState.IsValid)
            {            
                var tempId = ((int)Session["logged_user"]);
                var db = new AspdotNetSummerDBEntities();
                var user = (from e in db.users where e.user_id == tempId && e.password.Equals(u.password) select e).SingleOrDefault();
                if (user != null)
                {
                    var st = (from s in db.users where s.user_id == tempId select s).SingleOrDefault();
                    st.password = t.NewPassword;
                    db.SaveChanges();
                    return RedirectToAction("profile", "Driver");
                }
                else
                {
                    TempData["msg"] = "Invalid Password";
                    return View(u);
                }
            }

            return View();
        }

        public ActionResult TripOparation()
        {
            var tempData = "pending";
            var db = new AspdotNetSummerDBEntities();
            var trips = from p in db.trip_operations.ToList() where p.status.Equals(tempData) select p;

            return View(trips);
        }
        
        public ActionResult Confirmation(int id, trip_operations t, driver d)
        {
            var tempId = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var confirm = (from p in db.trip_operations where p.trip_id == id  select p).SingleOrDefault();
            var stt = (from s in db.drivers where s.user_id == tempId select s).SingleOrDefault();
            confirm.driver_id = stt.driver_id;
            confirm.status = "processing";
            db.SaveChanges();
            return RedirectToAction("TripOparation", "Driver");
            
        }

        public ActionResult CustomerProfile(int ID)
        {
            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.customers where p.customer_id == ID select p).SingleOrDefault();
           

            return View(user);
        }
        public ActionResult Dashboard()
        {
            var tempId = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.drivers where p.user_id == tempId select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempId select n).SingleOrDefault();
            ViewBag.email = usersinfo.email;
            ViewBag.pasword = usersinfo.password;

            return View(user);

        }

        public ActionResult ConfirmedTripOparation()
        {
            var tempId = ((int)Session["logged_user"]);
            var tempData = "processing";
            var db = new AspdotNetSummerDBEntities();
            var stt = (from s in db.drivers where s.user_id == tempId select s).SingleOrDefault();
            var trips = from p in db.trip_operations.ToList() where p.status.Equals(tempData) && p.driver_id.Equals(stt.driver_id) select p;

            return View(trips);
        }
        public ActionResult DeletePost(int id, trip_operations t, driver d)
        {
            var tempId = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var confirm = (from p in db.trip_operations where p.trip_id == id select p).SingleOrDefault();
            var stt = (from s in db.drivers where s.user_id == tempId select s).SingleOrDefault();
            confirm.driver_id =null;
            confirm.status = "pending";
            db.SaveChanges();
            return RedirectToAction("ConfirmedTripOparation", "Driver");

        }
    }
}