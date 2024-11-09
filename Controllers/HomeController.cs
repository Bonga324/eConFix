using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eConFix.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                //Try login with saved cookies
                if (cookie != null)
                {
                    ViewBag.userCookie = cookie.Value;
                    return View();
                }

                //Delete session data
                HttpCookie session = Request.Cookies["session"];
                if (session != null)
                {
                    session.Expires = DateTime.Now;
                    Response.Cookies.Add(session);
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "";


            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                //Try login with saved cookies
                if (cookie != null)
                {
                    ViewBag.userCookie = cookie.Value;
                    return View();
                }

                //Delete session data
                HttpCookie session = Request.Cookies["session"];
                if (session != null)
                {
                    session.Expires = DateTime.Now;
                    Response.Cookies.Add(session);
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult Contact()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                //Try login with saved cookies
                if (cookie != null)
                {
                    ViewBag.userCookie = cookie.Value;
                    return View();
                }

                //Delete session data
                HttpCookie session = Request.Cookies["session"];
                if (session != null)
                {
                    session.Expires = DateTime.Now;
                    Response.Cookies.Add(session);
                }
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }


        /// <summary>
        /// ////DELETE USER COOKIES
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            try
            {
                HttpCookie cookie = Request.Cookies["user"];
                cookie.Expires = DateTime.Now;
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}