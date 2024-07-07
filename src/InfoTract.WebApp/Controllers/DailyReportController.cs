using InfoTrack.WebApp.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InfoTract.WebApp.Controllers
{
    public class DailyReportController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashboardManager _dashboardManager;

        public DailyReportController(ILogger<HomeController> logger, IDashboardManager dashboardManager)
        {
            _logger = logger;
            _dashboardManager = dashboardManager;
        }

        public async Task<IActionResult> DailyReport()
        {
            var result = await _dashboardManager.GetDailyReport();
            return View(result);
        }
    }
}
