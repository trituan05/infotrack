using InfoTrack.WebApp.Business.Managers;
using InfoTract.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InfoTract.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardManager _dashboardManager;

        public HomeController(ILogger<HomeController> logger, IDashboardManager dashboardManager)
        {
            _logger = logger;
            _dashboardManager = dashboardManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keywords, string url)
        {
            List<int> index = await _dashboardManager.SearchByKeyword(keywords, url);
            string position = string.Join(",", index);
            ViewBag.Name = !string.IsNullOrEmpty(position) ? position : "0";
            return View();
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
}
