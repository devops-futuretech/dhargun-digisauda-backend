namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SAPEmailStatementStatsfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SAPEmailStatements", "SAPStatus", c => c.String());
            AlterColumn("dbo.SAPEmailStatements", "DocumentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SAPEmailStatements", "DocumentType", c => c.String());
            DropColumn("dbo.SAPEmailStatements", "SAPStatus");
        }
    }
}
