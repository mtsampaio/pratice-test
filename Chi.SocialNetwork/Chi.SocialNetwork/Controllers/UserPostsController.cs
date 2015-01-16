using Chi.SocialNetwork.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
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

            var user = CurrentUserHelper.GetCurrentUser(this.User.Identity as ClaimsIdentity);

            if (user == null)
            {
                return null;
            }

            foreach (var userPost in repository.GetUserPosts(id).OrderByDescending(p => p.PostDate))
            {
                var post = new UserPostDTO
                {
                    Id = userPost.Id,
                    Post = userPost.PostContent,
                    Files = this.repository.GetUserPostFiles(userPost.Id).Select(p => new PostFileDTO
                    {
                        Id = p.Id,
                        PostId = userPost.Id,
                        FileName = p.FileName,
                        Type = p.Type
                    }),
                    Liked = this.repository.GetUserPostLikes(userPost.Id).Any(p => p.User_Id == user.Id),
                    LikeCount = this.repository.GetUserPostLikes(userPost.Id).Count()
                };

                var comments = new List<PostCommentDTO>();
                foreach (var userComment in userPost.UserPostComments.OrderBy(p => p.CommentDate))
                {
                    comments.Add(new PostCommentDTO
                    {
                        Id = userComment.Id,
                        Comment = userComment.Comment,
                        Liked = this.repository.GetUserPostCommentLikes(userComment.Id).Any(p => p.User_Id == user.Id),
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
                    Files = null,
                    LikeCount = this.repository.GetUserPostLikes(userPost.Id).Count()
                });
        }
    }
}

