using Chi.SocialNetwork.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using Chi.SocialNetwork.Reports;
using System.Data;
using Chi.SocialNetwork.Models;

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

        [AllowAnonymous]
        public ActionResult Report()
        {
            var user = CurrentUserHelper.GetCurrentUser(this.User.Identity as ClaimsIdentity);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userInfo = new
            {
                PostsCount = this.rep.GetUserPosts(user.Id).Count(),
                FriendsCount = this.rep.GetUsers().Count(),
                LikesCount = this.rep.GetUserPosts(user.Id).ToList().Sum(p => this.rep.GetUserPostLikes(p.Id).ToList().Count()),
                PicturesCount = 0,
                VideosCount = 0,
            };

            var ds = new ReportDataSet();

            var userRow = ds.User.AddUserRow(user.Id, user.Name + " " + user.LastName, null, userInfo.LikesCount, userInfo.PostsCount, userInfo.PicturesCount,
                userInfo.VideosCount, userInfo.FriendsCount, (userInfo.FriendsCount - userInfo.LikesCount));

            foreach (var post in this.rep.GetUserPosts(user.Id))
            {
                var postRow = ds.Post.AddPostRow(userRow, post.PostContent, this.rep.GetUserPostLikes(post.Id).Count(), post.Id, post.PostDate);

                foreach (var comment in post.UserPostComments)
                {
                    ds.Comment.AddCommentRow(postRow, comment.Id, comment.Comment, comment.UserPostCommentLikes.Count, comment.User.Name + " " + comment.User.LastName, null);
                }
            }

            var rpt = new ReportClass();
            rpt.FileName = Server.MapPath("~/Reports/HobbiesReport.rpt");
            rpt.Load();
            rpt.SetDataSource(ds);

            var stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");
        }
    }
}
