namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ArName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "ArName");
        }
    }
}
