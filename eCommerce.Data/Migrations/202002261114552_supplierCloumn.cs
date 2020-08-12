namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplierCloumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SupplierID_ID", c => c.Int());
            CreateIndex("dbo.Products", "SupplierID_ID");
            AddForeignKey("dbo.Products", "SupplierID_ID", "dbo.Suppliers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID_ID", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierID_ID" });
            DropColumn("dbo.Products", "SupplierID_ID");
        }
    }
}
