using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eConFix.Models;

namespace eConFix.Controllers
{
    public class RegistersController : Controller
    {
        private ContextClass db = new ContextClass();

        // GET: Registers
        public ActionResult Index()
        {
            return View(db.Registers.ToList());
        }

        // GET: Registers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // GET: Registers/Create
        public ActionResult Create()
        {
            HttpCookie cookie = Request.Cookies["user"];
            //Try login with saved cookies
            if (cookie != null)
            {
                if (db.Registers.Find(cookie.Value) != null)
                {
                    return RedirectToAction("Index", "Bookings");
                }
                cookie.Expires = DateTime.Now;
                Response.SetCookie(cookie);
            }
            return View();
        }

        // POST: Registers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "emailAddress,name,surname,password,confirmPassword")] Register register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Temporarily save register model
                    Session["personalInfo"] = register;
                    return RedirectToAction("Create", "Payments");
                }
            }
            catch (Exception ex)
            {
                string emailAddress = register.emailAddress;
                register = db.Registers.Find(emailAddress);
                if (register!=null)
                {
                    //User already exists
                    return RedirectToAction("Login","Registers");
                }
                ViewBag.feedback = ex.Message;
            }
            return View(register);
        }

        // GET: Registers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // POST: Registers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emailAddress,name,surname,password,confirmPassword")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Entry(register).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(register);
        }

        // GET: Registers/Delete/5
        public ActionResult Delete(string emailAddress)
        {
            emailAddress = TempData["Id"].ToString();
            if (emailAddress == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(emailAddress);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        // POST: Registers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string emailAddress)
        {
            Payments payments = db.Payments.Find(emailAddress);
            if (payments != null)
            {
                db.Payments.Remove(payments);
            }

            Booking booking = new Booking();
            booking.DeleteMemberBookings(emailAddress);

            Register register = db.Registers.Find(emailAddress);
            db.Registers.Remove(register);
            db.SaveChanges();
            return RedirectToAction("Index","Services");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult Login()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                //Try login with saved cookies
                if (cookie!=null)
                {
                    //Check if user is still available
                    if (db.Registers.Find(cookie.Value) != null)
                    {
                        TempData["emailAddress"] = cookie.Value;
                        return RedirectToAction("Index", "Bookings");
                    }
                    cookie.Expires = DateTime.Now;
                    Response.SetCookie(cookie);
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login([Bind(Include = "emailAddress,password,saveInfo")] CustomerLogin login)
        {
            if (ModelState.IsValid)
            {
                Register register = new Register();
                string emailAddress = login.emailAddress;
                string password = login.password;

                //Try database coonection three tree time
                int attempts = 3;
                for (int i = 0; i < attempts; i++)
                {
                    try
                    {
                        register = db.Registers.Find(login.emailAddress);
                        break;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.db = ex.Message;
                    }
                }

                if (register != null)
                {
                    if (register.emailAddress == emailAddress && register.password == password)
                    {
                        //Access granted
                        if (login.saveInfo != null)
                        {
                            //Create cookie
                            HttpCookie cookies = new HttpCookie("user", register.emailAddress);
                            //Set expiry
                            cookies.Expires = DateTime.Now.AddMonths(1);
                            //Save cookies
                            HttpContext.Response.SetCookie(cookies);
                        }
                        else
                        {
                            //Create session cookie
                            HttpCookie session = new HttpCookie("session", register.emailAddress);
                            //Save cookies
                            HttpContext.Response.SetCookie(session);
                        }
                        return RedirectToAction("Index", "Bookings");
                    }
                    ViewBag.feedback = "Invalid password!";
                    return View();
                }
                ViewBag.feedback = "Invalid email address!";
            }
            return View();
        }

    }
}
