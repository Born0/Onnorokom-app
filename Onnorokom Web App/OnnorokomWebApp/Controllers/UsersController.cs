using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnnorokomWebApp.Models;

namespace OnnorokomWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly OnnoRokomDbContext _context;
        public const string SessionKeyRole = "_Role";
        public const string SessionKeyId = "_Id";
        public UsersController(OnnoRokomDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'OnnoRokomDbContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {

            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Role,Password")] User user)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            #region auth
            var cred = Auth();
            if (cred.Count == 0)
                return RedirectToAction("Login", "Login");
            #endregion Auth
            if (_context.Users == null)
            {
                return Problem("Entity set 'OnnoRokomDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #region Custom_Methods
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

        public IActionResult Signup()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup( User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                TempData["Id"]=user.Id;
                return RedirectToAction("Login","Login");
            }
            return View(user);
        }

        #endregion Custom_Methods
    }
}
