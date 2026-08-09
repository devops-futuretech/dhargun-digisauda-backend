namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCombinationInLiftingRequestDetailsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiftingRequestDetails", "SalesOrganizationId", c => c.Long(nullable: false));
            AddColumn("dbo.LiftingRequestDetails", "DistributionhannelId", c => c.Long(nullable: false));
            AddColumn("dbo.LiftingRequestDetails", "DivisionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LiftingRequestDetails", "DivisionId");
            DropColumn("dbo.LiftingRequestDetails", "DistributionhannelId");
            DropColumn("dbo.LiftingRequestDetails", "SalesOrganizationId");
        }
    }
}
