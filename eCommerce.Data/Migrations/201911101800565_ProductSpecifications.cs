namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductSpecifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSpecifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Title = c.String(),
                        Value = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSpecifications", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductSpecifications", new[] { "ProductID" });
            DropTable("dbo.ProductSpecifications");
        }
    }
}
