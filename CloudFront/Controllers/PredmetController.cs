using Microsoft.AspNetCore.Mvc;
using CloudFront.Mapper;
using CloudFront.Models;
using MongoDB.Bson;
namespace CloudFront.Controllers
{
    public class PredmetController : Controller
    {
        public IActionResult Index()
        {
            var s = Request.Cookies["Authentication"] as string;
            ViewBag.s = s;
            List<Predmet> list = new List<Predmet>();
            return View(list);
        }

        [HttpPost]
        public IActionResult Upis(string email, string predmet) {
            return View("Index");
        }
    }
}
