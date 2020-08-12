namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionIDinOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TransactionID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "TransactionID");
        }
    }
}
