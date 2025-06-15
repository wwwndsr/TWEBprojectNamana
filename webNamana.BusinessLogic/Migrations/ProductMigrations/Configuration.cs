namespace webNamana.BusinessLogic.Migrations.ProductMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<webNamana.BusinessLogic.DBModel.ProductContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true; // Enable automatic migrations
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(webNamana.BusinessLogic.DBModel.ProductContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
