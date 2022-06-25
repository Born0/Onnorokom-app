using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.View_Models;

namespace WebApplication1.Models
{
    public class OnnoRokomDbContext:DbContext
    {
        public OnnoRokomDbContext(DbContextOptions<OnnoRokomDbContext> options) :base(options)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<NoticeCounter> NoticesCounter { get; set; }
        public virtual DbSet<NoticeVisitedByUser> NoticesVisitedByUser { get; set; }
        public DbSet<WebApplication1.Models.View_Models.CredentialVM>? CredentialVM { get; set; }


    }
}
