namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTableForSaudaBookingConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaudaBookingConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.DiscountUsers", "StateId", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DiscountUsers", "StateId", c => c.Long(nullable: false));
            DropTable("dbo.SaudaBookingConfigurations");
        }
    }
}
