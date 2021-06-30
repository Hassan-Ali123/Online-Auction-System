namespace Sokuuhotu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payment_Registrations_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentReceivals",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        AmountInSubUnits = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        ImageLogUrl = c.String(),
                        OrderId = c.String(),
                        ProfileName = c.String(),
                        ProfileContact = c.String(),
                        ProfileEmail = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.RegistrationModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mobile = c.String(),
                        Email = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RegistrationModels");
            DropTable("dbo.PaymentReceivals");
        }
    }
}
