namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionMastertableTextlengthcolumnadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionMasters", "Textlength", c => c.String());
            //AddColumn("dbo.SaudaOrders", "QPSIdWithDiscount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaOrders", "QPSIdWithDiscount");
            //DropColumn("dbo.QuestionMasters", "Textlength");
        }
    }
}
