using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative_Project_Part_2.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Cumulative_Project_Part_2.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teachers
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(SearchKey);
            return View(Teachers);
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.Findteacher(id);


            return View(NewTeacher);
        }


        //POST : /Teacher/Delete/{id}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET : /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.Findteacher(id);


            return View(NewTeacher);
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string Name, string LastName, string EmpNum, DateTime HireDate, decimal Salary)
        {
            // Server-side validation
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("Name", "Teacher name is required.");
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                ModelState.AddModelError("LastName", "Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(EmpNum))
            {
                ModelState.AddModelError("EmpNum", "Employee number is required.");
            }

            if (HireDate == default(DateTime))
            {
                ModelState.AddModelError("HireDate", "Hire date is required.");
            }

            if (Salary <= 0)
            {
                ModelState.AddModelError("Salary", "Salary must be greater than zero.");
            }


            if (ModelState.IsValid)
            {
                //If no errors create 
                Teacher NewTeacher = new Teacher();
                NewTeacher.Name = Name;
                NewTeacher.LastName = LastName;
                NewTeacher.EmpNum = EmpNum;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                TeacherDataController controller = new TeacherDataController();
                controller.AddTeacher(NewTeacher);

                return RedirectToAction("List");
            }

            // If error back to list 
            return View("New");
        }


    }
}