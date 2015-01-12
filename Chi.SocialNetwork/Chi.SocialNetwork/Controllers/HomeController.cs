using Chi.SocialNetwork.Data;
using System.Web.Mvc;

namespace Chi.SocialNetwork.Controllers
{
    public class HomeController : Controller
    {

        Repository rep;

        public HomeController()
        {
            this.rep = new Repository();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "CHI Social Network";

            this.rep.InsertUser(new User());

            return View();
        }
    }
}
