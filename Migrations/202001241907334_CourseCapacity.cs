namespace MvcCourse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CourseCapacity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Course", "Capacity", c => c.Int(nullable: false));
            DropColumn("dbo.Course", "Credits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Course", "Credits", c => c.Int(nullable: false));
            DropColumn("dbo.Course", "Capacity");
        }
    }
}
