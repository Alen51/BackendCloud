
using System.ComponentModel.DataAnnotations;

namespace FrontUI.Models
{
    public class Profesor
    {
        

        public string ImeIPrezime { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public string Password { get; set; }

        public List<Predmet> PredmetList { get; set; } = new List<Predmet>();
    }
}
