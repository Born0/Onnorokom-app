using System.ComponentModel.DataAnnotations;
namespace OnnorokomWebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Required Field"), MinLength(3,ErrorMessage ="Minimum length is 3")]
        public string Password { get; set; }
    }
}
