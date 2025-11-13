namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvailableSizesToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AvailableSizes", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AvailableSizes");
        }
    }
}
