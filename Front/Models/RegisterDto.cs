using System.ComponentModel.DataAnnotations;

namespace Front.Models
{
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }



        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string Index { get; set; }

    }
}
