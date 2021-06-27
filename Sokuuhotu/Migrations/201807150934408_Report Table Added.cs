namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportAds",
                c => new
                    {
                        ReportAdId = c.Int(nullable: false, identity: true),
                        ReportMsg = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReportAdId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportAds", "ProductId", "dbo.Products");
            DropIndex("dbo.ReportAds", new[] { "ProductId" });
            DropTable("dbo.ReportAds");
        }
    }
}
