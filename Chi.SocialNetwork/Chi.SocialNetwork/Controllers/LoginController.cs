using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace Chi.SocialNetwork.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Title = "Welcome to Chi Social Network!";

            var userId = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            if (userId != null && string.IsNullOrEmpty(userId.Value) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}