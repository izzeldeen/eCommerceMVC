namespace eCommerce.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PromosUniqueCode : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "Users");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "UserClaims");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "UserLogins");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "UserRoles");
            RenameTable(name: "dbo.AspNetRoles", newName: "Roles");
            AlterColumn("dbo.Promoes", "Code", c => c.String(maxLength: 50));
            CreateIndex("dbo.Promoes", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Promoes", new[] { "Code" });
            AlterColumn("dbo.Promoes", "Code", c => c.String());
            RenameTable(name: "dbo.Roles", newName: "AspNetRoles");
            RenameTable(name: "dbo.UserRoles", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.UserLogins", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.UserClaims", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.Users", newName: "AspNetUsers");
        }
    }
}
