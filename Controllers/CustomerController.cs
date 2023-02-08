using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspDotNetSummerProject.Models;
using AspDotNetSummerProject.Models.Db;
using Newtonsoft.Json;

namespace AspDotNetSummerProject.Controllers
{
    [loged]
    public class CustomerController : Controller
    {
        
        // GET: Customer
        //Create user account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult create(user u,customer c)
        {
            if (ModelState.IsValid)
            {
                var db = new AspdotNetSummerDBEntities();
                u.user_type = "customer";
                db.users.Add(u);
                db.SaveChanges();
                var user = (from p in db.users where u.email.Equals(p.email) select p).SingleOrDefault();
                Session["logged_user"] = user.user_id;
                c.user_id = user.user_id;
                c.customer_name = "null";
                c.dob = new DateTime(1999,02,11);
                c.address = "null";
                c.phone = 0000;
                db.customers.Add(c);
                db.SaveChanges();
                return RedirectToAction("profile", "Customer");
            }
            else
            {
                return View(u);

            }       
        }


        //user dashboard
        public ActionResult CustomerHome()
        {
            if (Session["logged_user"] != null)
            {
                var tempuser_id = ((int)Session["logged_user"]);
                var db = new AspdotNetSummerDBEntities();
                var user = (from p in db.customers where p.user_id == tempuser_id select p).SingleOrDefault();
                ViewBag.name = user.customer_name;
                ViewBag.cid = user.customer_name;
                ViewBag.userId = user.user_id;
                var history = (from t in db.trip_operations where t.customer_id == user.customer_id select t).ToList();
                var current = DateTime.Now.Hour;

                if (current >= 12 && current <= 15)
                {

                     ViewBag.msg =  "Good noon";

                }
                else if (current >= 16 && current <= 17)
                {
                    ViewBag.msg = "Good Afternoon";
                }
                else if (current >= 18 && current <= 24)
                {
                    ViewBag.msg = "Good Evening";
                }
                else if (current >= 0 && current <= 5)
                {
                    ViewBag.msg = "Mid night";

                }
                else if (current >= 6 && current <= 11)
                {
                    ViewBag.msg = "Good morning";
                }

                return View(history);

            }
            return RedirectToAction("Login", "Home");
            
        }


        //test purpose
        public ActionResult info()
        {
            return View();
        }
        //public async Task<ActionResult> test()
        //{
        //    getLocationJSN model = new getLocationJSN();
        //    GeoHelper geoHelper = new GeoHelper();
        //    var result = await geoHelper.GetGeoInfo();
        //    model = JsonConvert.DeserializeObject<getLocationJSN>(result);

        //    return View(model);

        //}'
        [HttpGet]
        public ActionResult test()
        {
            return View();
        }
        [HttpPost]
        public ActionResult test(user s)
        {
            if (ModelState.IsValid)
            {
                return View();
            }
            else
            {
                return View(s);

            }
            
        }
        public JsonResult IsUserExists(string email)
        {
            var db = new AspdotNetSummerDBEntities();
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!db.users.Any(x => x.email == email),JsonRequestBehavior.AllowGet);
        }


        //search vechile
        [HttpGet]
        public ActionResult search()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult search(child_oftrip t)
        {
            if (ModelState.IsValid)
            {
                Session["end_location"] = t.end_location;
                Session["start_location"] = t.start_location;
                return RedirectToAction("booking", "Customer");
            }
            else
            {
                return View();
            }
            
            
        }
        [HttpGet]
        public ActionResult booking()
        {
            ViewBag.start_location = Session["start_location"];
            ViewBag.end_location = Session["end_location"];
            return View();
        }
        [HttpPost]
        public ActionResult booking(trip_operations t)
        {
            if (ModelState.IsValid)
            {
                ViewBag.start_location = Session["start_location"];
                ViewBag.end_location = Session["end_location"];
                var db = new AspdotNetSummerDBEntities();
                var tempuser_id = ((int)Session["logged_user"]);
                var cusomerinfo = (from p in db.customers where p.user_id == tempuser_id select p).SingleOrDefault();
                t.customer_id = cusomerinfo.customer_id;
                t.start_location = ((string)Session["start_location"]);
                t.end_location = ((string)Session["end_location"]);
                t.price = "10000";
                t.status = "pending";
                t.labour_id = "null";
                db.trip_operations.Add(t);
                db.SaveChanges();

                return RedirectToAction("CustomerHome", "Customer");
            }
            else
            {
                ViewBag.start_location = Session["start_location"];
                ViewBag.end_location = Session["end_location"];
                return View();
            }

        }


        //user profile
        public ActionResult profile()
        {
            var tempuser_id = ((int)Session["logged_user"]);


            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.customers where p.user_id == tempuser_id select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempuser_id select n).SingleOrDefault();
            ViewBag.email= usersinfo.email;
            ViewBag.pasword = usersinfo.password;

            return View(user);
        }

        //user edited oparetion
        [HttpGet]
        public ActionResult edit(user u, customer c)
        {
            var tempuser_id = ((int)Session["logged_user"]);
            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.customers where p.user_id == tempuser_id select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempuser_id select n).SingleOrDefault();
            ViewBag.email = usersinfo.email;
            ViewBag.password = usersinfo.password;
            //string datee = user.dob;
            //string datee = "2022-07-15";
            //ViewBag.date = DateTime.ParseExact(datee, "yyyy-MM-dd", null).ToShortDateString();
            ViewBag.date = user.dob.ToShortDateString();

            return View(user);
        }
        [HttpPost]
        public ActionResult edit(user u, customer c,trip_operations t)
        {
            
                AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
                var tempuser_id = ((int)Session["logged_user"]);
                var st = (from s in db.users where s.user_id == tempuser_id select s).SingleOrDefault();
                var stt = (from s in db.customers where s.user_id == tempuser_id select s).SingleOrDefault();
                //ViewBag.password = st.password;
                stt.customer_name = c.customer_name;
                stt.dob = c.dob;
                stt.address = c.address;
                stt.phone = c.phone;
                st.password = u.password;
                db.SaveChanges();
                return RedirectToAction("profile", "Customer");
    
        }
        [HttpGet]
        public ActionResult review(review r,int id)
        {
            var db = new AspdotNetSummerDBEntities();
            var reviewcheck = (from p in db.reviews where p.trip_id == id select p).SingleOrDefault();
            if (reviewcheck != null)
            {
                TempData["msg"] = "Already reviewed";
                return RedirectToAction("CustomerHome", "Customer");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult review(review r,trip_operations t,int id)
        {
            var db = new AspdotNetSummerDBEntities();
            
                    var data = (from p in db.trip_operations where p.trip_id == id select p).SingleOrDefault();
                    r.trip_id = id;
                    r.customer_id = data.customer_id;
                    r.review_date = DateTime.Now;
                    db.reviews.Add(r);
                    db.SaveChanges();
                    return RedirectToAction("CustomerHome", "Customer");

                

            }

        public ActionResult driver(driver d,int id)
        {
            var db = new AspdotNetSummerDBEntities();
            var result = (from p in db.drivers where p.driver_id == id select p).SingleOrDefault();
            return View(result);
        }

        public ActionResult delete(int id)
        {
            var db = new AspdotNetSummerDBEntities();
            var trip = (from p in db.trip_operations where p.trip_id == id select p).SingleOrDefault();
            db.trip_operations.Remove(trip);
            db.SaveChanges();
            return RedirectToAction("CustomerHome","Customer");
        }













        //logout
        public ActionResult Logout()
        {
            Session.Remove("logged_user");
            return RedirectToAction("Login","Home");
        }
    }
}