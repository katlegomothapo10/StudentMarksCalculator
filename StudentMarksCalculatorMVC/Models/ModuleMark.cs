using System.ComponentModel.DataAnnotations;

namespace StudentMarksCalculatorMVC.Models
{
    public class ModuleMark
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 8, ErrorMessage = "Module number must be between 1 and 8")]
        public int ModuleNumber { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Mark must be between 0 and 100")]
        [Display(Name = "Mark (%)")]
        public double Mark { get; set; }

        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }

        // Calculated properties
        public bool IsPassed => Mark >= 50;

        public string Grade
        {
            get
            {
                if (Mark >= 90) return "A+";
                if (Mark >= 80) return "A";
                if (Mark >= 70) return "B";
                if (Mark >= 60) return "C";
                if (Mark >= 50) return "D";
                return "F";
            }
        }

        public string GetStatusClass()
        {
            return IsPassed ? "text-success" : "text-danger";
        }
    }
}