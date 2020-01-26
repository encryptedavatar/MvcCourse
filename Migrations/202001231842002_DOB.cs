namespace MvcCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DOB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "DOB", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "DOB");
        }
    }
}
