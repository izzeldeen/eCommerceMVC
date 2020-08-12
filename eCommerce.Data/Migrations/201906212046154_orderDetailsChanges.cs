namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderDetailsChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ZipCode", c => c.String());
            AddColumn("dbo.Orders", "CustomerPhone", c => c.String());
            AddColumn("dbo.Orders", "PaymentMethod", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "DeliveryCharges", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "FinalAmmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderItems", "ItemPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "PaymentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "PaymentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.OrderItems", "ItemPrice");
            DropColumn("dbo.Orders", "FinalAmmount");
            DropColumn("dbo.Orders", "DeliveryCharges");
            DropColumn("dbo.Orders", "Discount");
            DropColumn("dbo.Orders", "PaymentMethod");
            DropColumn("dbo.Orders", "CustomerPhone");
            DropColumn("dbo.AspNetUsers", "ZipCode");
        }
    }
}
