using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eConFix.Models;

namespace eConFix.Controllers
{
    public class ServicesController : Controller
    {
        private ContextClass db = new ContextClass();

        // GET: Services
        public ActionResult Index()
        {
            return View(db.Services.ToList());
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "serviceId,serviceName,servicePrice")] Services services)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(services);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(services);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "serviceId,serviceName,servicePrice")] Services services)
        {
            if (ModelState.IsValid)
            {
                db.Entry(services).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(services);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Services services = db.Services.Find(id);
            db.Services.Remove(services);
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




        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////// P A R T I A L   V I E W S    A C T I O N S //////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [ChildActionOnly]
        public PartialViewResult _Bookings()
        {
            //Return Partial view name and object
            return PartialView("_Bookings", db.Bookings.ToList());
        }
        [ChildActionOnly]
        public PartialViewResult _Admins()
        {
            //Return Partial view name and object
            return PartialView("_Admins", db.Admins.ToList());
        }
        [ChildActionOnly]
        public PartialViewResult _Registered()
        {
            return PartialView("_Registered", db.Registers.ToList());
        }













        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////// A D M I N  A C T I O N S ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult DeleteMember(string emailAddress)
        {
            if (emailAddress!=null)
            {
                TempData["Id"] = emailAddress;
                return RedirectToAction("Delete","Registers");
            }
            return RedirectToAction("Index");
        }



        // GET: Admins/Delete/5
        public ActionResult DeleteAdmin(string userName)
        {
            Admin admin = db.Admins.Find(userName);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: Bookings/Delete/5
        public ActionResult DeleteBooking(int? id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
