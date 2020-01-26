using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcCourse.DAL;
using MvcCourse.Models;
using MvcCourse.ViewModels;

namespace MvcCourse.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Course
        //public ActionResult Index()
        public ActionResult Index(int? id, int? courseID)

        {
            //var courses = db.Courses.Include(c => c.Department);
            //return View(courses.ToList());

            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(i => i.LastName);
            viewModel.Courses = db.Courses;
            ////
            List<int> listAge = new List<int> { 1, 2, 3, 4 };
            listAge.Clear();
            foreach (Student s in db.Students)
                listAge.Add(s.Age);

            listAge.Sort();
            decimal smallest = listAge[0];
            decimal largest = listAge[listAge.Count() - 1];
            decimal average = (smallest + largest) / 2;
            ////
            if (courseID != null)
            {
                ViewBag.CourseID = courseID.Value;
                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                    ViewBag.NumberOfStudents = selectedCourse.Enrollments.Count();
                    ViewBag.MinAge = smallest;
                    ViewBag.MaxAge = largest;
                    ViewBag.AverageAge = average;
                    ViewBag.Instructor = selectedCourse.Instructors.First().FullName;

                }
                viewModel.Enrollments = selectedCourse.Enrollments;
            }

            ////
            //ViewBag.MinAge = smallest;
            //ViewBag.MaxAge = largest;
            //ViewBag.AverageAge = average;
            ////

            return View(viewModel);
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var studentList = from s in db.Students
                              select s;

            ViewBag.Data1 = studentList;
            /*           foreach (Enrollment e in enrollments)
                           if (e.CourseID == 1)
                           { }

                       //
                       ViewBag.TotalStudents = numstudents;
                       ViewBag.EnrollmentsViewBag = enrollments;

                       int[] listAge = new int[] { 1, 2, 3, 4 };

                       decimal smallest = listAge[0];
                       decimal largest = listAge[listAge.Count() - 1];
                       decimal average = (smallest + largest) / 2;
             */
            //Array.Sort(studentList);

            /*
            ViewBag.Enrollments = db.Enrollments;
            ViewBag.MinAge = db.Students;
            ViewBag.MaxAge = db.Students;
            ViewBag.AverageAge = db.Students;
            */

            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Capacity,DepartmentID")]Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseToUpdate = db.Courses.Find(id);
            if (TryUpdateModel(courseToUpdate, "",
               new string[] { "Title", "Capacity", "DepartmentID" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return View(courseToUpdate);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departments
                                   orderby d.Name
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
