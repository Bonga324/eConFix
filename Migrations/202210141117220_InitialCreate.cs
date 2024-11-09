namespace eConFix.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.userName);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        bookingId = c.Int(nullable: false, identity: true),
                        emailAddress = c.String(maxLength: 128),
                        serviceId = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.bookingId)
                .ForeignKey("dbo.Registers", t => t.emailAddress)
                .ForeignKey("dbo.Services", t => t.serviceId, cascadeDelete: true)
                .Index(t => t.emailAddress)
                .Index(t => t.serviceId);
            
            CreateTable(
                "dbo.Registers",
                c => new
                    {
                        emailAddress = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        surname = c.String(),
                        contact = c.String(),
                        password = c.String(),
                        confirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.emailAddress);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        serviceId = c.Int(nullable: false, identity: true),
                        serviceName = c.String(),
                        servicePrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.serviceId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        paymentId = c.Int(nullable: false, identity: true),
                        emailAddress = c.String(maxLength: 128),
                        cardNumber = c.String(),
                        name = c.String(),
                        cvc = c.Int(nullable: false),
                        zipCode = c.String(),
                    })
                .PrimaryKey(t => t.paymentId)
                .ForeignKey("dbo.Registers", t => t.emailAddress)
                .Index(t => t.emailAddress);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "emailAddress", "dbo.Registers");
            DropForeignKey("dbo.Bookings", "serviceId", "dbo.Services");
            DropForeignKey("dbo.Bookings", "emailAddress", "dbo.Registers");
            DropIndex("dbo.Payments", new[] { "emailAddress" });
            DropIndex("dbo.Bookings", new[] { "serviceId" });
            DropIndex("dbo.Bookings", new[] { "emailAddress" });
            DropTable("dbo.Payments");
            DropTable("dbo.Services");
            DropTable("dbo.Registers");
            DropTable("dbo.Bookings");
            DropTable("dbo.Admins");
        }
    }
}
