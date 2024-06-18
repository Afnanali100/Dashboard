using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantifyApp.Apis.Dtos;
using PlantifyControlPanel.ViewModels;
using System.Diagnostics;
using System.Text;

namespace PlantifyControlPanel.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
       
        public IActionResult Index(UserDto model)
        {

            string lastLoginTimeString = HttpContext.Session.GetString("LastLoginTime");
            DateTime lastLoginTime;
            if (!string.IsNullOrEmpty(lastLoginTimeString) && DateTime.TryParse(lastLoginTimeString, out lastLoginTime))
            {
              model.LogineTime = lastLoginTime;
            }
                return View(model);
        }

        public ActionResult PlantDiseasePieChart()
        {

            var plantDiseases = new List<PlantDisease>()
            {
            new PlantDisease { Name = "Powdery Mildew", Count = 20 },
            new PlantDisease { Name = "Leaf Spot", Count = 15 },
            new PlantDisease { Name = "Root Rot", Count = 10 },

             };


            return PartialView("_PlantDiseasePieChart",plantDiseases);
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