namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bidrequesttalechanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidRequests", "FirstPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BidRequests", "SecondPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BidRequests", "Price");
            DropColumn("dbo.BidRequests", "BidTimes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BidRequests", "BidTimes", c => c.Int(nullable: false));
            AddColumn("dbo.BidRequests", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.BidRequests", "SecondPrice");
            DropColumn("dbo.BidRequests", "FirstPrice");
        }
    }
}
