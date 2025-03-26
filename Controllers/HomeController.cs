using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrewMate.Models;

namespace CrewMate.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var postgreConnection = Environment.GetEnvironmentVariable("DATABASE_SETTING");

        Console.WriteLine($"Postgre Connection: {postgreConnection}");

        //return View();
        return !this.User.Identity.IsAuthenticated ? this.Redirect("~/identity/account/login") : View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
