namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderAndOrderDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderCode", c => c.String());
            AlterColumn("dbo.Orders", "CustomerName", c => c.String());
            AlterColumn("dbo.Orders", "Address", c => c.String());
            AlterColumn("dbo.Orders", "Phone", c => c.String());
            CreateIndex("dbo.OrderDetails", "ProductId");
            AddForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            DropColumn("dbo.OrderDetails", "ProductName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "ProductName", c => c.String());
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            AlterColumn("dbo.Orders", "Phone", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Orders", "Address", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Orders", "CustomerName", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Orders", "OrderCode");
        }
    }
}
