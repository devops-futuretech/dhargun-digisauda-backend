namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TickerColorColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickers", "ColorCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickers", "ColorCode");
        }
    }
}
