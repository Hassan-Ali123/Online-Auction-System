namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offsetaddinProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "UserTimeOffset", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "UserTimeOffset");
        }
    }
}
