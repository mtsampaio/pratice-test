using Chi.SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Chi.SocialNetwork.Controllers
{
    [Authorize]
    public class UserPostsController : ApiController
    {
        private Repository repository = new Repository();

        // GET: api/UserPosts
        public IEnumerable<UserPostDTO> GetUserPosts(int id)
        {
            var posts = new List<UserPostDTO>();

            foreach (var userPost in repository.GetUserPosts(id).OrderByDescending(p => p.PostDate))
            {
                var post = new UserPostDTO
                {
                    Id = userPost.Id,
                    Post = userPost.PostContent,
                    Type = userPost.ContentType,
                    LikeCount = this.repository.GetUserPostLikes(userPost.Id).Count()
                };

                var comments = new List<PostCommentDTO>();
                foreach (var userComment in userPost.UserPostComments.OrderBy(p => p.CommentDate))
                {
                    comments.Add(new PostCommentDTO
                    {
                        Id = userComment.Id,
                        Comment = userComment.Comment,
                        LikeCount = this.repository.GetUserPostCommentLikes(userComment.Id).Count(),
                        User = new UserDTO
                        {
                            Id = userComment.User.Id,
                            Name = userComment.User.Name + " " + userComment.User.LastName,
                            Portrait = ""
                        }
                    });
                }

                post.Comments = comments;
                posts.Add(post);
            }

            return posts;
        }

        // POST: api/UserPosts
        [ResponseType(typeof(UserPost))]
        public async Task<IHttpActionResult> PostUserPost(UserPost userPost)
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

            userPost.User_Id = user.Id;
            userPost.PostDate = DateTime.Now;

            await this.repository.InsertUserPostAsync(userPost);

            return CreatedAtRoute("DefaultApi", new { id = userPost.Id },
                new UserPostDTO
                {
                    Id = userPost.Id,
                    Post = userPost.PostContent,
                    Type = userPost.ContentType,
                    LikeCount = this.repository.GetUserPostLikes(userPost.Id).Count()
                });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}