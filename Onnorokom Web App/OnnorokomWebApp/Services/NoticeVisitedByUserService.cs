using System.Linq;
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
                _context.Add(ogData);
                await _context.SaveChangesAsync();
                return "Added";
            }
            return "Already Read";
        }

        public async Task<List<Notice>> GetNoticeVisitedByUser( int userId)
        {
            var data = _context.NoticesVisitedByUser.Where(x => x.UserId == userId).Select(x => x.NoticeId).ToList();

           List<Notice> visitednotices= new List<Notice>();
            foreach(var noticeId in data)
            {
                visitednotices.Add(_context.Notices.Where(x => x.Id == noticeId).FirstOrDefault());
            }
                                                                    
            return visitednotices;
        }


    }
}
