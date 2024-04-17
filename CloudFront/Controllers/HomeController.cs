using CloudFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StudentService.Interfaces;
using System.Diagnostics;

namespace CloudFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {/*
            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var token = "";
            var result = "";
            if (Request.Cookies["Authentication"] != "")
            {
                token= Request.Cookies["Authentication"] as string;
                result = serviceProxy.Authentcate(token).Result;
            }
           
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
                ViewBag.NotifyOnStart = TempData["Message"] as string;*/
            ViewBag.IsLoggedIn = false;
            return View();
        }

        public IActionResult Index2()
        {
            
            return View("Index");
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
