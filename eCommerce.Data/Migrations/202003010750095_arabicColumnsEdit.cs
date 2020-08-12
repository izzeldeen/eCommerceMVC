namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arabicColumnsEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ArDescription", c => c.String());
            DropColumn("dbo.Products", "ArDescreption");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ArDescreption", c => c.String());
            DropColumn("dbo.Products", "ArDescription");
        }
    }
}
