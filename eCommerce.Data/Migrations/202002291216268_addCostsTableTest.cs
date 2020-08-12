namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCostsTableTest : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductCosts", "ProductID");
            AddForeignKey("dbo.ProductCosts", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCosts", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductCosts", new[] { "ProductID" });
        }
    }
}
