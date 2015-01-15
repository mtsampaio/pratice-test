using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Chi.SocialNetwork.Data;
using Chi.SocialNetwork.Models;
using System.Security.Claims;

namespace Chi.SocialNetwork.Controllers
{
    public class UserPostCommentLikeController : ApiController
    {
        private Repository repository = new Repository();

        // POST: api/UserPostCommentLike
        [ResponseType(typeof(UserPostCommentLike))]
        public async Task<IHttpActionResult> PostUserPostCommentLike(UserPostCommentLike userPostCommentLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = CurrentUserHelper.GetCurrentUser(this.User.Identity as ClaimsIdentity);

            if (user == null)
            {
                return BadRequest(Properties.Resources.InvalidUserIdentity);
            }

            userPostCommentLike.User_Id = user.Id;

            await repository.InsertUserPostCommentLikeAsync(userPostCommentLike);

            return CreatedAtRoute("DefaultApi", new { id = userPostCommentLike.Id }, userPostCommentLike);
        }

        // DELETE: api/UserPostCommentLike/5
        [ResponseType(typeof(UserPostCommentLike))]
        public async Task<IHttpActionResult> DeleteUserPostCommentLike(int id)
        {
            repository.DeleteUserPostCommentLikeAsync(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}