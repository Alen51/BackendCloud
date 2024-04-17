using FrontUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StudentServiceStatefull.Interfaces;
using System.Diagnostics;

namespace FrontUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
                var token = "";
                token=Convert.ToString(Request.Cookies["Authentication"]); //?? "";
                var result = serviceProxy.Authentcate(token).Result; ;
                Console.WriteLine("Vrednost tokena:");
                Console.WriteLine(token);
                Console.WriteLine("/n");

                Console.WriteLine("Vrednost rezultata");
                Console.WriteLine(result);

                if (String.IsNullOrEmpty(token) || !result.Item1)
                {
                    ViewBag.IsLoggedIn = false;
                    ViewBag.Role = result.Item2;

                }
                else
                {
                    ViewBag.Role = result.Item2;
                    ViewBag.IsLoggedIn = true;
                }

                if (TempData["Message"] != null)
                    ViewBag.NotifyOnStart = TempData["Message"] as string;
            }
            catch
            {
                ViewBag.IsLoggedIn = false;
            }
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
