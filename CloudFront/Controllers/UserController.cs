using CloudFront.Mapper;
using CloudFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using StudentService.Interfaces;

namespace CloudFront.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public UserController()
        {

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
            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"),
                new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentService.Models.Student usr = new StudentService.Models.Student();
            usr = mapper.Map<StudentService.Models.Student>(user);
            var res = serviceProxy.Register(usr);
            var result = res.Result;
            if (result.Item1 == false)
            {
                TempData["Message"] = result.Item2;
                return RedirectToAction("Register");
            }

            return RedirectToAction("Login");

        }

        [HttpPost]
        public IActionResult SubmitLoginStudent(LoginDto user)
        {
            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentService.Models.Student usr = new StudentService.Models.Student();
            usr = mapper.Map<StudentService.Models.Student>(user);
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
            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentService.Models.Student usr = new StudentService.Models.Student();
            usr = mapper.Map<StudentService.Models.Student>(user);
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
            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            Models.Student usr = new Models.Student();
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

            var serviceProxy = ServiceProxy.Create<IStudentService>(new Uri("fabric:/BackendCloud/StudentService"), new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(1));
            var mapper = MappingProfile.Initialize();

            StudentService.Models.Student usr = new StudentService.Models.Student();
            usr = mapper.Map<StudentService.Models.Student>(user);
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
    }
}
