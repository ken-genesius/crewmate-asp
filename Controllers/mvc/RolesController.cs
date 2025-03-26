using CrewMate.Data;
using CrewMate.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrewMate.Controllers.mvc
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            IdentityRole role = new IdentityRole();
            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper();

            var result = await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var role = new RoleViewModel();
            var result = await _roleManager.FindByIdAsync(id);
            role.Name = result.Name;
            role.Id = result.Id;

            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, RoleViewModel model)
        {
            var checkIfExist = await _roleManager.RoleExistsAsync(model.Name);
            if(!checkIfExist)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                role.Name = model.Name;
                role.NormalizedName = model.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }

            return View(model);

        }
    }
}
