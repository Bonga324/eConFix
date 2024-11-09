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
    public class AdminsController : Controller
    {
        private ContextClass db = new ContextClass();

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userName,password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index","Services");
            }

            return View(admin);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if (ModelState.IsValid)
            {
                //Default admin login data
                if (admin.userName == "econfix" && admin.password == "econfix")
                {
                    //Access granted
                    return RedirectToAction("Index", "Services");
                }

                string username = admin.userName;
                string password = admin.password;
                admin = db.Admins.Find(admin.userName); 

                if (admin != null)
                {
                    if (admin.userName == username && admin.password == password)
                    {
                        //Access granted
                        return RedirectToAction("Index","Services");
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
