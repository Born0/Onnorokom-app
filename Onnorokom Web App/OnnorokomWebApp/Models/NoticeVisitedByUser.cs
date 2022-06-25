using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnnorokomWebApp.Models
{
    public class NoticeVisitedByUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int NoticeId { get; set; }


        [ForeignKey("NoticeId")]
        public virtual Notice Notice { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
