namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainImageUrladdedinproductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "MainImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "MainImageUrl");
        }
    }
}
