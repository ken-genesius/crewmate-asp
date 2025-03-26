using CrewMate.Data;
using CrewMate.Models;
using CrewMate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CrewMate.Controllers.mvc
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserRolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.UserRoles.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleViewModel model)
        {
            IdentityUserRole<string> userRole = new IdentityUserRole<string>();
            userRole.UserId = model.UserId;
            userRole.RoleId = model.RoleId;

            _context.Add(userRole);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId, string roleId)
        {
            var userRole = new UserRoleViewModel();
            IdentityUserRole<string> result = await _context.UserRoles.Where(ur => ur.UserId == userId && ur.RoleId == roleId).FirstOrDefaultAsync();
            if (result == null)
            {
                return NotFound();
            }
            userRole.UserId = result.UserId;
            userRole.RoleId = result.RoleId;

            return View(userRole);
        }

        // POST: SystemCodeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userId, string roleId)
        {
            IdentityUserRole<string> userRole = await _context.UserRoles.Where(ur => ur.UserId == userId && ur.RoleId == roleId).FirstOrDefaultAsync();
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
