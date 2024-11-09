using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Configuration;
using System.Net;

namespace eConFix.Models
{
    public class eConFix
    {
        public void SendMail(string to, string subject, string body, Register register, Booking booking)
        {
            if (body =="Payment")
            {
                body = $"{register.name}. \nYou have successfully registered with eConFix, \nYou can now login using your credentials to book a service.";
            }
            else
            {
                body = $"You have successfully booked a service with eConFix.\nYou requested: {booking.service} \nService cost{booking.price}";
            }
            string from = ConfigurationManager.AppSettings["Gmail"];
            try
            {
                using (MailMessage mail = new MailMessage(from, to, subject, body))
                {
                    mail.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["Gmailhost"], int.Parse(ConfigurationManager.AppSettings["Port"])))
                    {
                        smtp.Timeout = 500000;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Gmail"], ConfigurationManager.AppSettings["GmailPwd"]);
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception)
            {
                //Do nothing
            }
        }
    }
}