namespace Org.Cafh.Courseware.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUserRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUserRoles", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUserRoles", "ApplicationUser_Id");
        }
    }
}
