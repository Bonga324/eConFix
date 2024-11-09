using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using eConFix.Models;

namespace eConFix.Models
{
    public class ContextClass : DbContext
    {
        //Create database with the name "eConFixDB"
        public ContextClass() : base("eConFixDB")
        {

        }

        //Define tables of your models
        public DbSet<Services> Services { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}