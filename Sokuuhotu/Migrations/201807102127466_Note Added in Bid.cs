namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteAddedinBid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BidRequests", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BidRequests", "Note");
        }
    }
}
