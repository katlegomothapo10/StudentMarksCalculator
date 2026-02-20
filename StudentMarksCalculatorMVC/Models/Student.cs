using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudentMarksCalculatorMVC.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [Display(Name = "Student Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Student ID")]
        public string StudentId { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        // Navigation property
        public virtual ICollection<ModuleMark> ModuleMarks { get; set; }

        // Constructor
        public Student()
        {
            ModuleMarks = new List<ModuleMark>();
            RegistrationDate = DateTime.Now;
            LastUpdated = DateTime.Now;
            GenerateStudentId();
        }

        private void GenerateStudentId()
        {
            // Generate a simple student ID
            StudentId = "STU" + DateTime.Now.Ticks.ToString().Substring(10);
        }

        // Calculated properties
        public int TotalModules => ModuleMarks?.Count ?? 0;

        public int ModulesPassed
        {
            get
            {
                if (ModuleMarks == null) return 0;
                return ModuleMarks.Count(m => m.IsPassed);
            }
        }

        public double AverageMark
        {
            get
            {
                if (ModuleMarks == null || !ModuleMarks.Any()) return 0;
                return ModuleMarks.Average(m => m.Mark);
            }
        }

        public bool IsPassed => ModulesPassed >= 4; // 50% of 8 modules

        public string OverallResult
        {
            get
            {
                if (!ModuleMarks.Any()) return "Not Calculated";
                return IsPassed ? "PASSED" : "FAILED";
            }
        }

        public string ResultClass => IsPassed ? "text-success" : "text-danger";

        public double HighestMark => ModuleMarks != null && ModuleMarks.Any() ? ModuleMarks.Max(m => m.Mark) : 0;

        public double LowestMark => ModuleMarks != null && ModuleMarks.Any() ? ModuleMarks.Min(m => m.Mark) : 0;

        // Method to calculate pass rate
        public double PassRate
        {
            get
            {
                if (TotalModules == 0) return 0;
                return (double)ModulesPassed / TotalModules * 100;
            }
        }
    }
}