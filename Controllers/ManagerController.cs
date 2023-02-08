using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspDotNetSummerProject.Models;
using AspDotNetSummerProject.Models.Db;

namespace AspDotNetSummerProject.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Home()
        {
            var tempuser_id = ((int)Session["logged_user"]);


            var db = new AspdotNetSummerDBEntities();
            var user = (from p in db.managers where p.user_id == tempuser_id select p).SingleOrDefault();
            var usersinfo = (from n in db.users where n.user_id == tempuser_id select n).SingleOrDefault();
            ViewBag.email = usersinfo.email;
            ViewBag.pasword = usersinfo.password;

            return View(user);
        }
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(user u, manager m)
        {
            if (ModelState.IsValid)
            {
                var db = new AspdotNetSummerDBEntities();
                u.user_type = "manager";
                db.users.Add(u);
                db.SaveChanges();
                var user = (from p in db.users where u.email.Equals(p.email) select p).SingleOrDefault();
                Session["logged_user"] = user.user_id;
                m.user_id = user.user_id;
                ViewBag.manager_name = m.manager_name;
                ViewBag.address = m.address;
                ViewBag.phone = m.phone;
                db.managers.Add(m);
                db.SaveChanges();
                return RedirectToAction("Home", "manager");
            }
            else
            {
                return View(u);
            }

        }


        [HttpGet]
        public ActionResult edit()
        {
            var tempuser_id = ((int)Session["logged_user"]);
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var manager = (from n in db.managers where n.user_id == tempuser_id select n).FirstOrDefault();

            return View(manager);

        }

        [HttpPost]
        public ActionResult edit(user u, manager m)
        {
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var tempuser_id = ((int)Session["logged_user"]);
            var st = (from s in db.users where s.user_id == tempuser_id select s).SingleOrDefault();
            var stt = (from s in db.managers where s.user_id == tempuser_id select s).SingleOrDefault();
            stt.manager_name = m.manager_name;
            stt.address = m.address;
            stt.phone = m.phone;
            db.SaveChanges();
            return RedirectToAction("Home", "manager");
        }

        [HttpGet]
        public ActionResult changePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changePassword(user u, FormCollection form)
        {
            if (ModelState.IsValid)
            {            
                var tempId = ((int)Session["logged_user"]);
                var db = new AspdotNetSummerDBEntities();
                var user = (from e in db.users where e.user_id == tempId && e.password.Equals(u.password) select e).SingleOrDefault();
                if (user != null)
                {
                    var st = (from s in db.users where s.user_id == tempId select s).SingleOrDefault();
                    st.password = form["NewPassword"];
                    db.SaveChanges();
                    TempData["msg"] = "Password Changed";
                    return RedirectToAction("changePassword", "manager");
                }
                else
                {
                    TempData["msg"] = "Invalid Password";
                    return View(u);
                }
            }

            return View();
        }
        public ActionResult labourList()
        {
            var tempuser_id = ((int)Session["logged_user"]);
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var l = db.labours.ToList();
            return View(l);

        }
        public ActionResult tripOperation()
        {
            var temp_id = ((int)Session["Logged_user"]);
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var t = (from to in db.trip_operations where to.labour_id == "Null" select to).ToList();
            return View(t);
        }

        [HttpGet]
        public ActionResult add()
        {
            var temp_id = ((int)Session["Logged_user"]);
            return View();
        }

        [HttpPost]
        public ActionResult add(trip_operations t,int id)
        {
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var tempuser_id = ((int)Session["logged_user"]);
            var st = (from s in db.trip_operations where s.trip_id == id select s).FirstOrDefault();
            st.labour_id = t.labour_id;
            st.status = "confirmed";
            db.SaveChanges();
            return RedirectToAction("tripOperation", "manager");
        }
        
        public ActionResult delete(int id)
        {
            var temp_id = ((int)Session["Logged_user"]);
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var d = (from a in db.trip_operations where a.trip_id == id select a).FirstOrDefault();
            db.trip_operations.Remove(d);
            db.SaveChanges();
            return RedirectToAction("tripOperation", "manager");
        }
        public ActionResult comment()
        {
            var tempuser_id = ((int)Session["logged_user"]);
            AspdotNetSummerDBEntities db = new AspdotNetSummerDBEntities();
            var l = db.reviews.ToList();
            return View(l);
        }

    }
    
    
}