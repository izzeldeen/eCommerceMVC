namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentStuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountryCodes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Countryname = c.String(),
                        Code = c.Int(nullable: false),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TapUserId = c.String(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "FirstName", c => c.String());
            AddColumn("dbo.Users", "MiddleName", c => c.String());
            AddColumn("dbo.Users", "LastName", c => c.String());
            AddColumn("dbo.Users", "CountryCode", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "UserId", "dbo.Users");
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropColumn("dbo.Users", "CountryCode");
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "MiddleName");
            DropColumn("dbo.Users", "FirstName");
            DropTable("dbo.Payments");
            DropTable("dbo.CountryCodes");
        }
    }
}
