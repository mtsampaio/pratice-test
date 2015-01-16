using Chi.SocialNetwork.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Chi.SocialNetwork.Controllers
{
    public class UserPostLikeController : ApiController
    {
        private Repository repository = new Repository();

        // POST: api/UserPostLike
        [ResponseType(typeof(UserPostLike))]
        public async Task<IHttpActionResult> PostUserPostLike(UserPostLike userPostLike)
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

            userPostLike.User_Id = user.Id;

            await repository.InsertUserPostLikeAsync(userPostLike);

            return CreatedAtRoute("DefaultApi", new { id = userPostLike.Id }, userPostLike);
        }

        // DELETE: api/UserPostLike/5
        public IHttpActionResult DeleteUserPostLike(int id)
        {
            var user = CurrentUserHelper.GetCurrentUser(this.User.Identity as ClaimsIdentity);
            if (user == null)
            {
                return BadRequest(Properties.Resources.InvalidUserIdentity);
            }

            var like = this.repository.GetUserPostLikes(id).FirstOrDefault(p => p.User_Id == user.Id);

            if (like != null)
            {
                repository.DeleteUserPostLikeAsync(like.Id);
            }

            return Ok();
        }
    }
}