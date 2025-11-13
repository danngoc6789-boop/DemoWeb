namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelledDateToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CancelledDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CancelledDate");
        }
    }
}
