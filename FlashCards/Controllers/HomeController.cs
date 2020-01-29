using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlashCards.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ViewModels.ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Decks");
        }
    }
}
