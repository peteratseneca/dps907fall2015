namespace Lab6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserNameproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Instruments", "UserName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Instruments", "UserName");
        }
    }
}
