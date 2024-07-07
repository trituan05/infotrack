using InfoTrack.WebApp.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InfoTract.WebApp.Controllers
{
    public class WeeklyReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardManager _dashboardManager;

        public WeeklyReportController(ILogger<HomeController> logger, IDashboardManager dashboardManager)
        {
            _logger = logger;
            _dashboardManager = dashboardManager;
        }

        public async Task<IActionResult> WeeklyReport()
        {
            var result = await _dashboardManager.GetWeeklyReport();
            return View(result);
        }
    }
}
