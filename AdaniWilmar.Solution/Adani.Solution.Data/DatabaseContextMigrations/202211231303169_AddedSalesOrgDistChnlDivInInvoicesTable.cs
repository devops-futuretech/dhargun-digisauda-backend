namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSalesOrgDistChnlDivInInvoicesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceDetails", "SalesOrganizationId", c => c.Long(nullable: false));
            AddColumn("dbo.InvoiceDetails", "DistributionChannelId", c => c.Long(nullable: false));
            AddColumn("dbo.InvoiceDetails", "DivisionId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceDetails", "DivisionId");
            DropColumn("dbo.InvoiceDetails", "DistributionChannelId");
            DropColumn("dbo.InvoiceDetails", "SalesOrganizationId");
        }
    }
}
