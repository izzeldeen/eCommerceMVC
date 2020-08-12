namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class displayseqno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "DisplaySeqNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "DisplaySeqNo");
        }
    }
}
