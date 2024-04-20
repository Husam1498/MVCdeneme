using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Controllers
{
    [Authorize(Roles = "admin,yetkili", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]//rolu admin manager gibi olanlar erişebilir
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
