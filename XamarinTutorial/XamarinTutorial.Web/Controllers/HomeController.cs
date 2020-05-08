using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XamarinTutorial.Web.Models;
using XamarinTutorial;
using Xamarin.Forms;
using Ooui.AspNetCore;
using Xamarin.Forms.StyleSheets;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;

namespace XamarinTutorial.Web.Controllers
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

            var page = new NavigationPage(new MainPage());
            var element =  page.GetOouiElement();
            return new ElementResult(element, "NoteTaker+");

            /*Assembly CurrentAssembly = Assembly.GetExecutingAssembly();
            Stream EmbeddedResourceStream = CurrentAssembly.GetManifestResourceStream("XamarinTutorial.Web.Content.Styles.MainPage.css");
            
            using (var reader = new StreamReader(EmbeddedResourceStream))
            {
                StyleSheet styleSheet = StyleSheet.FromReader(reader);
                page.Resources.Add(styleSheet);
                
            }*/

            //StyleSheet styleSheet = StyleSheet.FromResource
            //    ("XamarinTutorial.Web/Content/Styles/MainPage.css",page.GetType().Assembly);

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
