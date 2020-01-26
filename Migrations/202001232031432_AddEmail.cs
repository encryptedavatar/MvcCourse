namespace MvcCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "Email");
        }
    }
}
