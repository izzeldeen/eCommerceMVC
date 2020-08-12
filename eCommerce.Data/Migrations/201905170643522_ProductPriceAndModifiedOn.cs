namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductPriceAndModifiedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Products", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Products", "ThumbnailPictureID", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.ProductPictures", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Pictures", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Comments", "ModifiedOn", c => c.DateTime());
            DropColumn("dbo.Products", "ActualAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ActualAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Comments", "ModifiedOn");
            DropColumn("dbo.Pictures", "ModifiedOn");
            DropColumn("dbo.ProductPictures", "ModifiedOn");
            DropColumn("dbo.Products", "ModifiedOn");
            DropColumn("dbo.Products", "ThumbnailPictureID");
            DropColumn("dbo.Products", "Price");
            DropColumn("dbo.Categories", "ModifiedOn");
        }
    }
}
