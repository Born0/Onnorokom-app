using WebApplication1.Models;

namespace OnnorokomWebApp.Services
{
    public class NoticeCounterService
    {
        readonly OnnoRokomDbContext _context;
        public NoticeCounterService(OnnoRokomDbContext context)
        {
            _context = context;
        }
        public async Task<string> NoticeReadCounted(int noticeId)
        {
            var ogNoticeCounter= _context.NoticesCounter.Where(x=>x.NoticeId==noticeId).FirstOrDefault();
            if(ogNoticeCounter!=null)
            {
                ogNoticeCounter.Counter++;
                _context.Update(ogNoticeCounter);
                await _context.SaveChangesAsync();
                return "Updated";
            }
            else
            {
                NoticeCounter noticeCounter=new NoticeCounter() { NoticeId = noticeId, Counter=1 };
                _context.Add(noticeCounter);
                await _context.SaveChangesAsync();
                return "Added";
            }
            
        }
    }
}
