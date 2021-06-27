namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccepetedColuumnAddedinBidRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidRequests", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BidRequests", "Accepted");
        }
    }
}
