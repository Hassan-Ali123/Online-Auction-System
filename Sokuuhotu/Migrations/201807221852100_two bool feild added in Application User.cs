namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twoboolfeildaddedinApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AsCompany", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "AsUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AsUser");
            DropColumn("dbo.AspNetUsers", "AsCompany");
        }
    }
}
