using CrewMate.Data;
using CrewMate.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrewMate.Controllers.mvc
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            IdentityUser user = new IdentityUser();
            user.UserName = model.UserName;
            user.NormalizedUserName = model.UserName.ToUpper();
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.EmailConfirmed = true;
            user.PhoneNumber = model.PhoneNumber;
            user.PhoneNumberConfirmed = true;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
