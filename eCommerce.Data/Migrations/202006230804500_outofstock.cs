namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outofstock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "isOutOfStock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "isOutOfStock");
        }
    }
}
