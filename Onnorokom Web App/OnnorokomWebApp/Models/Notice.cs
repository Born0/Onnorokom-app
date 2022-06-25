using System.ComponentModel.DataAnnotations;
namespace OnnorokomWebApp.Models
{
    public class Notice
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public string Title { get; set; }
        public string? Body { get; set; }
    
    }
}
