using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.View_Models;

namespace OnnorokomWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly OnnoRokomDbContext _context;
        public const string SessionKeyRole = "_Name";
        public const string SessionKeyId = "_Id";

        public LoginController(OnnoRokomDbContext context)
        {
            _context = context;
        }

     
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password")] CredentialVM credentialVM)
        {
            if (ModelState.IsValid)
            {
                var ogData = await _context.Users.Where(x => x.Id == credentialVM.Id && x.Password == credentialVM.Password).FirstOrDefaultAsync();

                if (ogData != null)
                {
                    HttpContext.Session.SetString(SessionKeyRole, ogData.Role);
                    HttpContext.Session.SetInt32(SessionKeyId, ogData.Id);

                    return RedirectToAction("Index", "Notices");
                }
                else
                    return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

      
    }
}
