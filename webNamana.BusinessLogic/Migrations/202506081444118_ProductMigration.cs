namespace webNamana.BusinessLogic.Migrations.ProductMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductEntities", "ProductImage", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductEntities", "ProductImage", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
