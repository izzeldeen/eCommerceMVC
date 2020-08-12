namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userIDinOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CustomerID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CustomerID");
        }
    }
}
