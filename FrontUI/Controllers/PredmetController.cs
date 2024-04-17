using FrontUI.Mapper;
using FrontUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using PredmetService.Interfaces;

namespace FrontUI.Controllers
{
    public class PredmetController : Controller
    {
        public IActionResult Index()
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.GetAllPredmets(Request.Cookies["Authentication"] as string).Result;
            List<PredmetDto> predmets = new List<PredmetDto>();
            foreach(var predmet in result)
            {
                PredmetDto predmet1 = new PredmetDto();
                predmet1.Predmet=mapper.Map<Predmet>(predmet.Predmet);
                predmet1.Upisan = predmet.Upisan;
                predmets.Add(predmet1);
            } 
            
            var s = Request.Cookies["Authentication"] as string;
            ViewBag.s = s;

            return View(predmets);
        }

        
        public IActionResult Upis(string predmet)
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.AddStudentToPredmet(Request.Cookies["Authentication"] as string, predmet).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Ispis(string predmet)
        {
            var serviceProxy = ServiceProxy.Create<IPredmetService>(new Uri("fabric:/BackendCloud/PredmetService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            var result = serviceProxy.RemoveStudentFromPredmet(Request.Cookies["Authentication"] as string, predmet).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Index", "Home");
            }
            return View("Index");
        }
    }
}
