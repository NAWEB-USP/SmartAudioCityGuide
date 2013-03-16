namespace SmartAudioCityGuide.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, storeType: "mediumtext"),
                        userName = c.String(nullable: false, storeType: "mediumtext"),
                        password = c.String(nullable: false, storeType: "mediumtext"),
                        authenticate = c.Int(nullable: false),
                        hash = c.String(storeType: "mediumtext"),
                        typeOfBlindness = c.Int(nullable: false),
                        idFacebook = c.String(storeType: "mediumtext"),
                        acessTokenFacebook = c.String(storeType: "mediumtext"),
                        phoneId = c.String(storeType: "mediumtext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        longitude = c.Double(nullable: false),
                        latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        locationsId = c.Int(nullable: false),
                        typeOfCommentsId = c.Int(nullable: false),
                        description = c.String(storeType: "mediumtext"),
                        archiveDescription = c.String(storeType: "mediumtext"),
                        isText = c.Boolean(nullable: false),
                        sound = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Codes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        code = c.String(storeType: "mediumtext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserLocations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        windowsPhoneId = c.String(storeType: "mediumtext"),
                        latitude = c.Double(nullable: false),
                        longitude = c.Double(nullable: false),
                        requestTime = c.DateTime(nullable: false),
                        hash = c.String(storeType: "mediumtext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(storeType: "mediumtext"),
                        eMail = c.String(storeType: "mediumtext"),
                        country = c.String(storeType: "mediumtext"),
                        city = c.String(storeType: "mediumtext"),
                        phone = c.String(storeType: "mediumtext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.TypeOfComments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(storeType: "mediumtext"),
                        description = c.String(storeType: "mediumtext"),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TypeOfComments");
            DropTable("dbo.Contacts");
            DropTable("dbo.UserLocations");
            DropTable("dbo.Codes");
            DropTable("dbo.Comments");
            DropTable("dbo.Locations");
            DropTable("dbo.Users");
        }
    }
}
