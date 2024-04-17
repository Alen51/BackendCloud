using Front.Mapper;
using Front.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using PredmetService.Interfaces;


namespace Front.Controllers
{
    public class PredmetController : Controller
    {
        public IActionResult Index()
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.GetAllPredmets().Result;
            List<Predmet> predmetiList=mapper.Map<List<Models.Predmet>>(result);
            var s = Request.Cookies["Authentication"] as string;
            ViewBag.s = s;
            
            return View(predmetiList);
        }

        [HttpPost]
        public IActionResult Upis(string email, string predmet)
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.AddStudentToPredmet(email,predmet).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Ispis(string email, string predmet)
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.RemoveStudentFromPredmet(email, predmet).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }
    }
}
