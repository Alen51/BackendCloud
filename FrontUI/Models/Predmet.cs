using System.ComponentModel.DataAnnotations;

namespace FrontUI.Models
{
    public class Predmet
    {
        [Required]
        public string ImePredmeta { get; set; }

        public string emailProfesora { get; set; }

        public Dictionary<string, Student> StudentList { get; set; } = new Dictionary<string, Student>();

        public Dictionary<string, int> OcenaList { get; set; } = new Dictionary<string, int>();
    }
}
