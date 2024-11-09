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
    public class BookingsController : Controller
    {
        private ContextClass db = new ContextClass();

        // GET: Bookings
        public ActionResult Index()
        {
            try
            {
                //Feedback for booking exeptions
                if (TempData["error"]!=null)
                {
                    ViewBag.error = TempData["error"].ToString();
                }
                List<Booking> bookings = new List<Booking>();

                var list = (from b in db.Bookings
                            join r in db.Registers on b.emailAddress equals r.emailAddress
                            join s in db.Services on b.serviceId equals s.serviceId
                            select new { b.bookingId, r.name, s.serviceName, s.servicePrice }).ToList();

                foreach (var item in list)
                {
                    Booking obj = new Booking();
                    obj.bookingId = item.bookingId;
                    obj.emailAddress = item.name;
                    obj.service = item.serviceName;
                    obj.price = item.servicePrice;
                    bookings.Add(obj);
                }

                //Try login with saved cookies
                HttpCookie cookie = Request.Cookies["user"];
                if (cookie != null)
                {
                    //Check if the user still exists
                    if (db.Registers.Find(cookie.Value) != null)
                    {
                        ViewBag.userCookie = cookie.Value;
                        return View(bookings.ToList());
                    }
                    cookie.Expires = DateTime.Now;
                    Response.SetCookie(cookie);
                }

                HttpCookie session = Request.Cookies["session"];
                if (session != null)
                {
                    ViewBag.userCookie = session.Value;
                    return View(bookings.ToList());
                }
                return RedirectToAction("Login", "Registers");
            }
            catch (Exception)
            {
                return RedirectToAction("Login","Registers");
            }
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                //Try login with saved cookies
                if (cookie != null)
                {
                    ViewBag.emailAddress = cookie.Value;
                    ViewBag.serviceId = new SelectList(db.Services, "serviceId", "serviceName");
                    ViewBag.servicePrice = new SelectList(db.Services, "serviceId", "servicePrice");
                    return View();
                }
                HttpCookie session = Request.Cookies["session"];
                Register register = db.Registers.Find(session.Value);

                ViewBag.emailAddress = register.emailAddress;
                ViewBag.serviceId = new SelectList(db.Services, "serviceId", "serviceName");
                ViewBag.servicePrice = new SelectList(db.Services, "serviceId", "servicePrice");
            }
            catch (Exception)
            {
                return RedirectToAction("Login","Registers");
            }
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "bookingId,emailAddress,serviceId,service,price")] Booking booking)
        {
            try
            {
                booking.service = booking.GetServiceName();
                booking.price = booking.GetPrice();
                if (booking.service != null && booking.price != 0)
                {
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    Models.eConFix eConFix = new Models.eConFix();
                    eConFix.SendMail(booking.emailAddress,"eConFix Booking","Booking",(Register)null,booking);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("Sequence contains no elements"))
                {
                    TempData["error"] = "All fields are required.";
                    return RedirectToAction("Index", "Bookings");
                }
                //Bookings Index feedback
                TempData["error"] = "Something went wrong!,  Try loging out and log back in.";
                return RedirectToAction("Index","Bookings");
            }

            ViewBag.emailAddress = new SelectList(db.Registers, "emailAddress", "name", booking.emailAddress);
            ViewBag.serviceId = new SelectList(db.Services, "serviceId", "serviceName", booking.serviceId);
            ViewBag.servicePrice = new SelectList(db.Services, "serviceId", "servicePrice");

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.emailAddress = new SelectList(db.Registers, "emailAddress", "name", booking.emailAddress);
            ViewBag.serviceId = new SelectList(db.Services, "serviceId", "serviceName", booking.serviceId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "bookingId,emailAddress,serviceId,price")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.emailAddress = new SelectList(db.Registers, "emailAddress", "name", booking.emailAddress);
            ViewBag.serviceId = new SelectList(db.Services, "serviceId", "serviceName", booking.serviceId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }









        ///
        ///
        ////////////////////////////////////////////////////
        ///

        public ActionResult Price(int? Id)
        {
            return View();
        }
    }
}
