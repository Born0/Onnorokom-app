using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnnorokomWebApp.Models;
using OnnorokomWebApp.Models.View_Models;
using OnnorokomWebApp.Services;

namespace OnnorokomWebApp.Controllers
{
    public class DashboardController : Controller
    {
        #region Declaration
        private readonly OnnoRokomDbContext _context;
        public const string SessionKeyRole = "_Role";
        public const string SessionKeyId = "_Id";
        NoticeCounterService noticeCounterService;
        NoticeVisitedByUserService noticeVisitedByUserService;

        #endregion Declaration

        public DashboardController(OnnoRokomDbContext context)
        {
            _context = context;
            noticeCounterService = new NoticeCounterService(_context);
            noticeVisitedByUserService = new NoticeVisitedByUserService(_context);
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            int userId = Int32.Parse(cred[1]);    // setting ID 
            string role = cred[0];


            var data = await _context.Notices.ToListAsync();
            if (data.Count > 0)              
            {
                if (ViewBag.Role == "ADMIN")        // Only for Admin
                {
                    List<NoticeViewVM> list = new List<NoticeViewVM>();
                    foreach (var notice in data)
                    {
                        list.Add(new NoticeViewVM()
                        {
                            Id = notice.Id,
                            Title = notice.Title,
                            Body = notice.Body,
                            Count = await noticeCounterService.GetNoticeReadCount(notice.Id)
                        });
                    }
                    return View("AdminIndex", list);
                }
                else
                {
                    var visitedList =await noticeVisitedByUserService.GetNoticeVisitedByUser(userId);   //getting Visited items
                    data = (List<Notice>)data.Except(visitedList).ToList(); // removing visited item from full lsit
                    data.AddRange(visitedList); // adding visited item at the end

                    return View(data);
                }
                
            }
            // for Users
            
            return View("Index","No Data");
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            int userId = Int32.Parse(cred[1]);    // setting ID 
            string role = cred[0];

            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (notice == null)
            {
                return NotFound();
            }

            if (role != "ADMIN") // Visit counter only for user
            {
                var counterDetails = await noticeCounterService.NoticeReadCounted(notice.Id);
                var visitDetails = await noticeVisitedByUserService.NoticeVisitedByUser(notice.Id, userId);
            }

            return View(notice);
        }

        public IActionResult Create()
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] Notice notice)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {
                _context.Add(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            if (id == null )
            {
                return NotFound();
            }

            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Notice notice)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            if (id != notice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(notice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Dashboard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");

            var notice = await _context.Notices.FindAsync(id);
            if (notice != null)
            {
                _context.Notices.Remove(notice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NoticeExists(int id)
        {
          return (_context.Notices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private List<string> Auth()
        {
            try
            {
                ViewBag.Role = HttpContext.Session.GetString(SessionKeyRole);
                int userId = (int)HttpContext.Session.GetInt32(SessionKeyId);   ///
                return new List<string> { ViewBag.Role, userId.ToString() };
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }
}
