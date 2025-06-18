namespace webNamana.BusinessLogic.Migrations.TrainingMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainingMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainingEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainingName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        DurationMinutes = c.Int(nullable: false),
                        DifficultyLevel = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TrainingEntities");
        }
    }
}
