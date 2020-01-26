﻿using MvcCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCourse.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
        public IEnumerable<Student> Students { get; set; }
    }
}