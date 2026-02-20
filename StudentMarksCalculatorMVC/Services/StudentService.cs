using StudentMarksCalculatorMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentMarksCalculatorMVC.Services
{
    public class StudentService : IStudentService
    {
        private static List<Student> _students = new List<Student>();
        private static int _nextStudentId = 1;
        private static int _nextModuleId = 1;

        public List<Student> GetAllStudents()
        {
            return _students.OrderByDescending(s => s.RegistrationDate).ToList();
        }

        public Student? GetStudentById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public void AddStudent(Student student)
        {
            student.Id = _nextStudentId++;
            student.LastUpdated = DateTime.Now;

            // Generate module marks if not present
            if (student.ModuleMarks == null)
            {
                student.ModuleMarks = new List<ModuleMark>();
            }

            _students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = GetStudentById(student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.LastUpdated = DateTime.Now;
            }
        }

        public void DeleteStudent(int id)
        {
            var student = GetStudentById(id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }

        public void AddModuleMark(int studentId, ModuleMark mark)
        {
            var student = GetStudentById(studentId);
            if (student != null)
            {
                mark.Id = _nextModuleId++;
                mark.StudentId = studentId;

                if (student.ModuleMarks == null)
                {
                    student.ModuleMarks = new List<ModuleMark>();
                }

                student.ModuleMarks.Add(mark);
                student.LastUpdated = DateTime.Now;
            }
        }

        public Dictionary<string, object> GetStatistics()
        {
            var stats = new Dictionary<string, object>();

            if (!_students.Any())
            {
                stats["TotalStudents"] = 0;
                stats["TotalPassed"] = 0;
                stats["TotalFailed"] = 0;
                stats["PassRate"] = 0.0;
                stats["AverageMark"] = 0.0;
                stats["HighestMark"] = 0.0;
                stats["LowestMark"] = 0.0;
                stats["TotalModules"] = 0;
                return stats;
            }

            var allMarks = _students.SelectMany(s => s.ModuleMarks).ToList();

            stats["TotalStudents"] = _students.Count;
            stats["TotalPassed"] = _students.Count(s => s.IsPassed);
            stats["TotalFailed"] = _students.Count(s => !s.IsPassed && s.ModuleMarks.Any());
            stats["PassRate"] = (double)_students.Count(s => s.IsPassed) / _students.Count * 100;
            stats["AverageMark"] = allMarks.Any() ? allMarks.Average(m => m.Mark) : 0.0;
            stats["HighestMark"] = allMarks.Any() ? allMarks.Max(m => m.Mark) : 0.0;
            stats["LowestMark"] = allMarks.Any() ? allMarks.Min(m => m.Mark) : 0.0;
            stats["TotalModules"] = allMarks.Count;

            return stats;
        }

        public bool StudentExists(int id)
        {
            return _students.Any(s => s.Id == id);
        }
    }
}