namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Supplier_ID", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "Supplier_ID" });
            RenameColumn(table: "dbo.Products", name: "Supplier_ID", newName: "SupplierID");
            AlterColumn("dbo.Products", "SupplierID", c => c.Int(nullable: true));
            CreateIndex("dbo.Products", "SupplierID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierID" });
            AlterColumn("dbo.Products", "SupplierID", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "SupplierID", newName: "Supplier_ID");
            CreateIndex("dbo.Products", "Supplier_ID");
            AddForeignKey("dbo.Products", "Supplier_ID", "dbo.Suppliers", "ID");
        }
    }
}
