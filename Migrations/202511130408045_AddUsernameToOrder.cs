namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsernameToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Username", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Username");
        }
    }
}
