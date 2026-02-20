using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentMarksCalculatorMVC.Models;
using StudentMarksCalculatorMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentMarksCalculatorMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student
        public IActionResult Index()
        {
            var students = _studentService.GetAllStudents();
            return View(students);
        }

        // GET: Student/Details/5
        public IActionResult Details(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name")] Student student)
        {
            if (ModelState.IsValid)
            {
                _studentService.AddStudent(student);
                TempData["SuccessMessage"] = $"Student {student.Name} created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/AddMarks/5
        public IActionResult AddMarks(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }

            // Check if marks already exist
            if (student.ModuleMarks.Count >= 8)
            {
                TempData["InfoMessage"] = "This student already has all 8 modules entered.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var model = new ModuleMark
            {
                StudentId = id,
                ModuleNumber = student.ModuleMarks.Count + 1
            };

            return View(model);
        }

        // POST: Student/AddMarks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMarks(ModuleMark moduleMark)
        {
            if (ModelState.IsValid)
            {
                var student = _studentService.GetStudentById(moduleMark.StudentId);

                // Check if module number is valid
                if (moduleMark.ModuleNumber > 8)
                {
                    ModelState.AddModelError("ModuleNumber", "Module number cannot exceed 8");
                    return View(moduleMark);
                }

                // Check if module mark already exists
                if (student.ModuleMarks.Any(m => m.ModuleNumber == moduleMark.ModuleNumber))
                {
                    ModelState.AddModelError("ModuleNumber", $"Module {moduleMark.ModuleNumber} already has a mark entered");
                    return View(moduleMark);
                }

                _studentService.AddModuleMark(moduleMark.StudentId, moduleMark);

                if (student.ModuleMarks.Count >= 8)
                {
                    TempData["SuccessMessage"] = "All 8 modules have been entered!";
                    return RedirectToAction(nameof(Details), new { id = moduleMark.StudentId });
                }

                TempData["SuccessMessage"] = $"Module {moduleMark.ModuleNumber} added successfully!";

                // Redirect to add next module
                return RedirectToAction(nameof(AddMarks), new { id = moduleMark.StudentId });
            }
            return View(moduleMark);
        }

        // GET: Student/Edit/5
        public IActionResult Edit(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _studentService.UpdateStudent(student);
                TempData["SuccessMessage"] = "Student updated successfully!";
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public IActionResult Delete(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _studentService.GetStudentById(id);
            _studentService.DeleteStudent(id);
            TempData["SuccessMessage"] = $"Student {student.Name} deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Statistics
        public IActionResult Statistics()
        {
            var stats = _studentService.GetStatistics();
            return View(stats);
        }

        // GET: Student/Print/5
        public IActionResult Print(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
    }
}