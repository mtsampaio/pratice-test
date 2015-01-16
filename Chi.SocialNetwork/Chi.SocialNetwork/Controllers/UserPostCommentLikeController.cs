using Chi.SocialNetwork.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Description;

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
        public IHttpActionResult DeleteUserPostCommentLike(int id)
        {
            var user = CurrentUserHelper.GetCurrentUser(this.User.Identity as ClaimsIdentity);
            if (user == null)
            {
                return BadRequest(Properties.Resources.InvalidUserIdentity);
            }

            var like = this.repository.GetUserPostCommentLikes(id).FirstOrDefault(p => p.User_Id == user.Id);

            if (like != null)
            {
                repository.DeleteUserPostCommentLikeAsync(like.Id);
            }

            return Ok();
        }
    }
}