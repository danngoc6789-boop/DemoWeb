namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSupportRequests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Email = c.String(),
                        Message = c.String(nullable: false),
                        Status = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.OrderDetails", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderDetails", "UnitPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.SupportRequests");
        }
    }
}
