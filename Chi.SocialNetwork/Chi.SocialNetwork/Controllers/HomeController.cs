using Chi.SocialNetwork.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.IO;

namespace Chi.SocialNetwork.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Repository rep;
        User loggedUser = null;

        public HomeController()
        {
            this.rep = new Repository();
        }

        [AllowAnonymous]
        public ActionResult Index(int? id)
        {
            if (User == null || User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Login");
            }

            // checks for the first time access if there is any logged user
            if (loggedUser == null)
            {
                var loggedUserId = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

                if (loggedUserId != null && string.IsNullOrEmpty(loggedUserId.Value) == false)
                {
                    loggedUser = this.rep.GetUserById(Convert.ToInt32(loggedUserId.Value));
                }
            }

            var userId = id ?? loggedUser.Id;

            var user = userId == loggedUser.Id ? loggedUser : this.rep.GetUserById(userId) ?? loggedUser;

            ViewBag.LoggedUser = loggedUser;
            ViewBag.UserId = user.Id;
            ViewBag.FullName = string.Format("{0} {1}", user.Name, user.LastName);
            ViewBag.Name = user.Name;
            ViewBag.About = user.About;
            ViewBag.IsOwner = userId == loggedUser.Id;

            return View();
        }

        public ActionResult PostFile(int id)
        {
            var file = this.rep.GetUserPostFile(id);
            if (file != null)
            {
                var dir = Server.MapPath("/App_Data");
                var path = Path.Combine(dir, file.FileName);
                return base.File(path, "image/jpeg");
            }
                return base.File("", file.Type);
        }

        protected override void Dispose(bool disposing)
        {
            this.rep.Dispose();
            base.Dispose(disposing);
        }
    }
}
