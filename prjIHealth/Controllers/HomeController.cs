using CoreMVC_SignalR_Chat.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using prjiHealth.Models;
using prjIHealth.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiHealth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHealthContext _context;
        private readonly IHubContext<ChatHub> _countHub;

        public HomeController(ILogger<HomeController> logger, IHealthContext context, IHubContext<ChatHub> countHub)
        {
            _logger = logger;
            _context = context;
            _countHub = countHub;
        }
        public async Task Send(string msg, string id)
        {
            await _countHub.Clients.All.SendAsync("AddMsg", $"{id}：{msg}");
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            var memberEdit = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
            var loginUser = JsonSerializer.Deserialize<TMember>(memberEdit);

            return Content($"{loginUser.FUserName}", "text/plain", System.Text.Encoding.UTF8);
        }
        //[HttpPost]
        //public IActionResult Login(CLoginViewModel vModel)
        //{
        //    var q = _context.TMembers.FirstOrDefault(tm => tm.FUserName == vModel.fUserName);
        //    if (q != null)
        //    {
        //        if (q.FPassword == vModel.fPassword)
        //        {
        //            return RedirectToRoute(new { controller = "Home", action = "會員專區ViewDemo" });
        //        }
        //    }
        //    return View();
        //}

        public IActionResult 一般ViewDemo()
        {
            return View();
        }
        public IActionResult 會員專區ViewDemo()
        {
            return View();
        }
        public IActionResult 教練專區ViewDemo()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult signalRChat() {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
