namespace FLS.Server.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FLS.Server.Data.FLSDataEntities>
    {
        public Configuration()
        {
            //To add new migration setting, execute: "add-migration InitialCreate -ProjectName FLS.Server.Data" in Package Manager Console
            //see also http://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx

            AutomaticMigrationsEnabled = false;

            Database.SetInitializer<FLSDataEntities>(new CreateDatabaseIfNotExists<FLSDataEntities>());
            //Database.SetInitializer<FLSDataEntities>(new DropCreateDatabaseIfModelChanges<FLSDataEntities>());
            //Database.SetInitializer<FLSDataEntities>(new DropCreateDatabaseAlways<FLSDataEntities>());
            //Database.SetInitializer<FLSDataEntities>(new SchoolDBInitializer());
        }

        protected override void Seed(FLS.Server.Data.FLSDataEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
