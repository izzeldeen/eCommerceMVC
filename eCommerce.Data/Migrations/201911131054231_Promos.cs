namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Promoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Code = c.String(),
                        PromoType = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidTill = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Orders", "PromoID", c => c.Int());
            CreateIndex("dbo.Orders", "PromoID");
            AddForeignKey("dbo.Orders", "PromoID", "dbo.Promoes", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "PromoID", "dbo.Promoes");
            DropIndex("dbo.Orders", new[] { "PromoID" });
            DropColumn("dbo.Orders", "PromoID");
            DropTable("dbo.Promoes");
        }
    }
}
