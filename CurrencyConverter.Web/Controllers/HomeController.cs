using CurrencyConverter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CurrencyConverter.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Amount"] = "10";
            ViewData["Currency"] = "USD";

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Convert()
        {
            var amount = HttpContext.Request.Form["Amount"];
            var currency = HttpContext.Request.Form["Currency"];

            var conversionService = new ConversionService();
            var convertedAmount = conversionService.Convert(amount, currency);

            ViewData["Amount"] = amount;
            ViewData["Currency"] = currency;
            ViewData["ConvertedAmount"] = convertedAmount;

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
