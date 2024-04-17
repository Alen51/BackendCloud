using System.ComponentModel.DataAnnotations;

namespace Front.Models
{
    public class Predmet
    {
        [Required]
        public string ImePredmeta { get; set; }

        public string emailProfesora { get; set; }

        public Dictionary<string, Student> StudentList { get; set; }

        public Dictionary<string, int> OcenaList { get; set; }
    }
}
