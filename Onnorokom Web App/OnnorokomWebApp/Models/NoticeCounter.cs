using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnnorokomWebApp.Models
{
    public class NoticeCounter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NoticeId { get; set; }
        [Required]
        public int Counter { get; set; }

        [ForeignKey("NoticeId")]
        public virtual Notice Notice { get; set; }

    }
}
