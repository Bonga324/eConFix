namespace eConFix.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<eConFix.Models.ContextClass>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "eConFix.Models.ContextClass";
        }

        protected override void Seed(eConFix.Models.ContextClass context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
