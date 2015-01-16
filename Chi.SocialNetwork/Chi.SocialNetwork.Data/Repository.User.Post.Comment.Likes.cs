using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        public IQueryable<UserPostCommentLike> GetUserPostCommentLikes(int postCommentId)
        {
            return this.entities.UserPostCommentLikes.Where(p => p.UserPostComment_Id == postCommentId);
        }

        /// <summary>
        /// Inserts a new post like asynchrosnouly into Chi Social Network database.
        /// </summary>
        /// <param name="post">The post the user liked</param>
        /// <returns></returns>
        public async Task<UserPostCommentLike> InsertUserPostCommentLikeAsync(UserPostCommentLike like)
        {
            like = this.entities.UserPostCommentLikes.Add(like);
            await this.SaveChangesAsync();
            return like;
        }

        /// <summary>
        /// Deletes a post like asynchronously from Chi Social Network database.
        /// </summary>
        /// <param name="post">The post the user disliked</param>
        public void DeleteUserPostCommentLikeAsync(int userPostCommentLikeId)
        {
            var postLike = this.entities.UserPostCommentLikes.FirstOrDefault(p => p.Id == userPostCommentLikeId);
            if (postLike != null)
            {
                this.entities.UserPostCommentLikes.Remove(postLike);
                this.SaveChanges();
            }
        }
    }
}
