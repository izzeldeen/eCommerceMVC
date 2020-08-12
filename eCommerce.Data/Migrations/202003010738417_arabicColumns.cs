namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arabicColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ArName", c => c.String());
            AddColumn("dbo.Products", "ArSummary", c => c.String());
            AddColumn("dbo.Products", "ArDescreption", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ArDescreption");
            DropColumn("dbo.Products", "ArSummary");
            DropColumn("dbo.Products", "ArName");
        }
    }
}
