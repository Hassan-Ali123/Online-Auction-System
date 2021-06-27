namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeildAddedinContacttable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "PostedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contacts", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "IsRead");
            DropColumn("dbo.Contacts", "PostedDate");
        }
    }
}
