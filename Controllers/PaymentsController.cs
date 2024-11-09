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
    public class PaymentsController : Controller
    {
        private ContextClass db = new ContextClass();

        // GET: Payments/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Create","Registers");
            }
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "paymentId,emailAddress,cardNumber,name,cvc,expiry")] Payments payments,string mm, string yy)
        {
            //Create a list of validation issues
            List<String> errors = new List<string>();
            string currentYear = DateTime.Now.ToString("yy");
            if (int.Parse(mm) > 12)
            {
                errors.Add("MM invalid");
            }
            if (int.Parse(yy) < int.Parse(DateTime.Now.ToString("yy")))
            {
                errors.Add("YY invalid, Card expired");
            }


            // Check for your Session Value
            if (Session["personalInfo"] != null)
            {
                //If there is no validation issues
                if (errors.Count == 0)
                {
                    // Your Session value exists - cast your Session object to the appropriate type
                    Register registers = (Register)Session["personalInfo"];
                    payments.emailAddress = registers.emailAddress;
                    payments.expiry = $"{mm}/{yy}";

                    db.Registers.Add(registers);
                    db.Payments.Add(payments);
                    db.SaveChanges();
                    Models.eConFix eConFix = new Models.eConFix();
                    eConFix.SendMail(registers.emailAddress,"Successfully payment.","Payment",registers ,(Booking)null);
                    return RedirectToAction("Index","Bookings");
                }
                ViewBag.errors = errors;
                return View(payments);
            }
            errors.Add("Something went wrong!, Try again later");
            //return errors feedback
            ViewBag.errors = errors;
            return View(payments);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
