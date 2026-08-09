namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalesOrgDistChannelInTarget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserCustomerSalesTargets", "SalesOrganizationId", c => c.Long());
            AddColumn("dbo.UserCustomerSalesTargets", "DistributionChannelId", c => c.Long());
            AddColumn("dbo.UserCustomerSaudaTargets", "SalesOrganizationId", c => c.Long());
            AddColumn("dbo.UserCustomerSaudaTargets", "DistributionChannelId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserCustomerSaudaTargets", "DistributionChannelId");
            DropColumn("dbo.UserCustomerSaudaTargets", "SalesOrganizationId");
            DropColumn("dbo.UserCustomerSalesTargets", "DistributionChannelId");
            DropColumn("dbo.UserCustomerSalesTargets", "SalesOrganizationId");
        }
    }
}
