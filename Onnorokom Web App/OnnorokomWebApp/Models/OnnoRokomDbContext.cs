using Microsoft.EntityFrameworkCore;

namespace OnnorokomWebApp.Models
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


    }
}
