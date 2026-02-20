using StudentMarksCalculatorMVC.Models;
using System.Collections.Generic;

namespace StudentMarksCalculatorMVC.Services
{
    public interface IStudentService
    {
        List<Student> GetAllStudents();
        Student? GetStudentById(int id);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
        void AddModuleMark(int studentId, ModuleMark mark);
        Dictionary<string, object> GetStatistics();
        bool StudentExists(int id);
    }
}