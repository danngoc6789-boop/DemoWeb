namespace DemoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGoldPrice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoldPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Karat = c.String(),
                        Purity = c.String(),
                        BuyPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Gender");
            DropTable("dbo.GoldPrices");
        }
    }
}
