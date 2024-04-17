using ClosedXML.Excel;
using FrontUI.Mapper;
using FrontUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StudentServiceStatefull.Interfaces;
using System.Text;

namespace FrontUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginStudent()
        {
            LoginDto loginUserDTO = new LoginDto();
            if (TempData["Message"] != null)
            {
                ViewBag.NotifyOnStart = TempData["Message"] as string;
            }
            return View(loginUserDTO);
        }

        public IActionResult LoginProfesor()
        {
            LoginDto loginUserDTO = new LoginDto();
            if (TempData["Message"] != null)
            {
                ViewBag.NotifyOnStart = TempData["Message"] as string;
            }
            return View(loginUserDTO);
        }

        public IActionResult Register()
        {
            RegisterDto registerUserDTO = new RegisterDto();
            if (TempData["Message"] != null)
            {
                ViewBag.NotifyOnStart = TempData["Message"] as string;
            }
            return View(registerUserDTO);
        }

        [HttpPost]
        public IActionResult SubmitRegister(RegisterDto user)
        {
            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"),
                new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentServiceStatefull.Models.Student usr = new StudentServiceStatefull.Models.Student();
            usr = mapper.Map<StudentServiceStatefull.Models.Student>(user);
            var res = serviceProxy.Register(usr);
            var result = res.Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Register");
            }

            return RedirectToAction("LoginStudent");

        }

        [HttpPost]
        public IActionResult SubmitLoginStudent(LoginDto user)
        {
            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentServiceStatefull.Models.Student usr = new StudentServiceStatefull.Models.Student();
            usr = mapper.Map<StudentServiceStatefull.Models.Student>(user);
            var result = serviceProxy.LogIn(user.Email, user.Password).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = "Email or password incorrect";
                return RedirectToAction("LoginStudent");
            }
            TempData["Message"] = "Welcome " + user.Email.Split('@')[0];

            Response.Cookies.Append("Authentication", result.Item2);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult SubmitLoginProfesor(LoginDto user)
        {
            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentServiceStatefull.Models.Student usr = new StudentServiceStatefull.Models.Student();
            usr = mapper.Map<StudentServiceStatefull.Models.Student>(user);
            var result = serviceProxy.LogInProfesor(user.Email, user.Password).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = "Email or password incorrect";
                return RedirectToAction("LoginProfesor");
            }
            TempData["Message"] = "Welcome " + user.Email.Split('@')[0];

            Response.Cookies.Append("Authentication", result.Item2);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            Student usr = new Student();
            var result = serviceProxy.GetProfile(Request.Cookies["Authentication"] as string).Result;
            usr = mapper.Map<Models.Student>(result.Item2);
            if (result.Item1 == false)
            {
                return RedirectToAction("Index", "Home");
            }
            if (TempData["Message"] != null)
                ViewBag.NotifyOnStart = TempData["Message"] as string;

            return View(usr);
        }

        [HttpPost]
        public IActionResult ModifyProfile(Models.Student user)
        {

            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentServiceStatefull.Models.Student usr = new StudentServiceStatefull.Models.Student();
            usr = mapper.Map<StudentServiceStatefull.Models.Student>(user);
            if (serviceProxy.Authentcate(Request.Cookies["Authentication"] as string).Result.Item1 == false)
            {
                TempData["Message"] = "Your session has expired";
                return RedirectToAction("Index", "Home");
            }
            var result = serviceProxy.ModifyProfile(usr).Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Profile", "User");
            }

            TempData["Message"] = result.Item2;
            return RedirectToAction("Profile", "User");
        }

        public IActionResult Izvestaj()
        {
            var serviceProxy = ServiceProxy.Create<IStudentServiceStatefull>(new Uri("fabric:/BackendCloud/StudentServiceStatefull"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();
            string email = Request.Cookies["Authentication"] as string;
            var result = serviceProxy.Izvestaj(email).Result;
            if (result.Item1 == false)
            {

                return RedirectToAction("Index", "Home");
            }
            var i = mapper.Map<Izvestaj>(result.Item2);

            var pdf = new ChromePdfRenderer();
            StringBuilder sb = new StringBuilder();
            sb.Append("<h1>This is a heading</h1>");
            foreach (var item in i.ocene) {
                sb.Append(String.Format("Ime predmeta:{0} Ocena:{1}",item.ImePredmeta,item.Ocena));
                    }
            sb.Append('\n');
            sb.Append(String.Format("Sredja ocena:{0}", i.SrednjaOcena));
            PdfDocument doc = pdf.RenderHtmlAsPdf(sb.ToString()
                );
            
            doc.SaveAs(@"E:\myPdf.pdf");

            return RedirectToAction("Index", "Home");
        }
    }
}
