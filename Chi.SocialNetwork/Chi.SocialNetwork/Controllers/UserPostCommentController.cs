using Chi.SocialNetwork.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Chi.SocialNetwork.Controllers
{
    [Authorize]
    public class UserPostCommentController : ApiController
    {
        private Repository repository = new Repository();

        // POST: api/UserPostComment
        [ResponseType(typeof(PostCommentDTO))]
        public async Task<IHttpActionResult> PostUserPostComment(UserPostComment userPostComment)
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

            userPostComment.User_Id = user.Id;
            userPostComment.CommentDate = DateTime.Now;

            await repository.InsertUserPostCommentAsync(userPostComment);

            var dto = new PostCommentDTO
            {
                Id = userPostComment.Id,
                Comment = userPostComment.Comment,
                PostId = userPostComment.UserPost_Id,
                LikeCount = this.repository.GetUserPostCommentLikes(userPostComment.Id).Count(),
                User = new UserDTO {
                    Id = user.Id,
                    Name = user.Name + " " + user.LastName,
                    Portrait = ""
                }
            };

            return CreatedAtRoute("DefaultApi", new { id = userPostComment.Id }, dto);
        }
    }
}