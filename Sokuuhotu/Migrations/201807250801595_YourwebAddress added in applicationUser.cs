namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YourwebAddressaddedinapplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "YourWebAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "YourWebAddress");
        }
    }
}