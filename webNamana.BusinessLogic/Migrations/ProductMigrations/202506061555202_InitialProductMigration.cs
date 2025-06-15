namespace webNamana.BusinessLogic.Migrations.ProductMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialProductMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductImage = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductEntities");
        }
    }
}
