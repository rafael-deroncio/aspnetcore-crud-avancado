using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrudAvancado
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}