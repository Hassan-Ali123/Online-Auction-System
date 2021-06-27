namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class auuctionend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AuctionEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AuctionEndDate");
        }
    }
}
