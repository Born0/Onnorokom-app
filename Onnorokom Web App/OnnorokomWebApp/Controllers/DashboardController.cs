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
        private readonly OnnoRokomDbContext _context;
        public const string SessionKeyRole = "_Role";
        public const string SessionKeyId = "_Id";
        NoticeCounterService noticeCounterService;
        NoticeVisitedByUserService noticeVisitedByUserService;

        public DashboardController(OnnoRokomDbContext context)
        {
            _context = context;
            noticeCounterService = new NoticeCounterService(_context);
            noticeVisitedByUserService = new NoticeVisitedByUserService(_context);
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            ViewBag.Role = HttpContext.Session.GetString(SessionKeyRole);
            var data = await _context.Notices.ToListAsync();
            if (ViewBag.Role=="ADMIN")
            {
                if (data.Count > 0)
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
                return View("AdminIndex", "NO DATA");
            }
            
            return View(data);
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Role = HttpContext.Session.GetString(SessionKeyRole);
            int userId = (int)HttpContext.Session.GetInt32(SessionKeyId);   ///need modification
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Where(m => m.Id == id).FirstOrDefaultAsync();
            if (notice == null)
            {
                return NotFound();
            }
            var counterDetails= await noticeCounterService.NoticeReadCounted(notice.Id);

           
            var visitDetails = await noticeVisitedByUserService.NoticeVisitedByUser(notice.Id, userId);

            return View(notice);
        }

        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] Notice notice)
        {
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
    }
}
