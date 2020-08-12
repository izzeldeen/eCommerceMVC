namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editColName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Products", name: "SupplierID_ID", newName: "Supplier_ID");
            RenameIndex(table: "dbo.Products", name: "IX_SupplierID_ID", newName: "IX_Supplier_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Products", name: "IX_Supplier_ID", newName: "IX_SupplierID_ID");
            RenameColumn(table: "dbo.Products", name: "Supplier_ID", newName: "SupplierID_ID");
        }
    }
}
