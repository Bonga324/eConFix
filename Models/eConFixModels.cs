using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using eConFix.Models;

namespace eConFix.Models
{
    public class Services
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serviceId { get; set; }
        [Required]
        [Display(Name ="Service Name")]
        public string serviceName { get; set; }
        [Required]
        [Display(Name ="Service Price")]
        public decimal servicePrice { get; set; }

    }


    public class Register
    {
        [Key]
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }
        [Required]
        [Display(Name ="Name")]
        public string name { get; set; }
        [Required]
        public string surname { get; set; }
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
    public class CustomerLogin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool? saveInfo { get; set; }
    }





    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bookingId { get; set; }
        public string emailAddress { get; set; }
        public virtual Register Register { get; set; }
        public int serviceId { get; set; }
        public virtual Services Services { get; set; }
        [Display(Name ="Service")]
        public string service { get; set; }
        [Display(Name ="Price")]
        public decimal price { get; set; }


        /************ LINQ METHODS ***********/

        ContextClass db = new ContextClass();
        public string GetServiceName()
        {
            var service = (from s in db.Services
                     where s.serviceId == serviceId
                     select s.serviceName).Single();
            return service;
        }

        public decimal GetPrice()
        {
            var price = (from s in db.Services
                     where s.serviceId == serviceId
                     select s.servicePrice).Single();
            return price;
        }

        public void DeleteMemberBookings(string email)
        {
            List<Booking> list = new List<Booking>();
            var bookingIdList = (from b in db.Bookings
                                 where b.emailAddress == email
                                 select b).ToList();
            foreach (var item in bookingIdList)
            {
                db.Bookings.Remove(item);
                db.SaveChanges();
            }
        }
    }



    public class Payments
    {
        [Key]
        public string emailAddress { get; set; }
        public virtual Register Register { get; set; }
        [MinLength(16),MaxLength(16)]
        public string cardNumber { get; set; }
        public string name { get; set; }
        public int cvc { get; set; }
        public string expiry { get; set; }
    }

    public class Admin
    {
        [Key]
        [Display(Name ="Username")]
        public string userName { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string password { get; set; }
    }
}