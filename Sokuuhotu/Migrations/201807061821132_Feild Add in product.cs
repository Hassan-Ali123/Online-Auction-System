namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeildAddinproduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AuctionEnd", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AuctionEnd");
        }
    }
}
