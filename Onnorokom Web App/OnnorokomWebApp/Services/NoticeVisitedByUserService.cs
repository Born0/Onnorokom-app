using OnnorokomWebApp.Models;

namespace OnnorokomWebApp.Services
{
    public class NoticeVisitedByUserService
    {
        readonly OnnoRokomDbContext _context;
        public NoticeVisitedByUserService(  OnnoRokomDbContext context)
        {
            _context = context;
        }
        public async Task<string> NoticeVisitedByUser(int noticeId, int userId)
        {
            var ogData = _context.NoticesVisitedByUser.Where(x => x.NoticeId == noticeId && x.UserId == userId).FirstOrDefault();
            if(ogData == null)
            {
                ogData=new NoticeVisitedByUser();
                ogData.UserId = userId;
                ogData.NoticeId = noticeId;
                ogData.IsRead = true;
                _context.Update(ogData);
                await _context.SaveChangesAsync();
                return "Updated";
            }
            return "Already Read";
        }
    }
}
