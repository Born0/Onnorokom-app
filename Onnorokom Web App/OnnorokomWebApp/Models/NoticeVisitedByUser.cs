using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
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
        public Notice Notice { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
