namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagesColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Images", c => c.String(maxLength: 2000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Images");
        }
    }
}
