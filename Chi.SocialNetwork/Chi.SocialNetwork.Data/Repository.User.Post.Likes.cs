using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        public IQueryable<UserPostLike> GetUserPostLikes(int postId)
        {
            return this.entities.UserPostLikes.Where(p => p.UserPost_Id == postId);
        }

        /// <summary>
        /// Inserts a new post like asynchrosnouly into Chi Social Network database.
        /// </summary>
        /// <param name="post">The post the user liked</param>
        /// <returns></returns>
        public async Task<UserPostLike> InsertUserPostLikeAsync(UserPostLike post)
        {
            post = this.entities.UserPostLikes.Add(post);
            await this.SaveChangesAsync();
            return post;
        }

        /// <summary>
        /// Deletes a post like asynchronously from Chi Social Network database.
        /// </summary>
        /// <param name="post">The post the user disliked</param>
        public async void DeleteUserPostLikeAsync(int userPostLikeId)
        {
            var postLike = await this.entities.UserPostLikes.FirstOrDefaultAsync(p => p.Id == userPostLikeId);
            if (postLike != null)
            {
                this.entities.UserPostLikes.Remove(postLike);
                await this.SaveChangesAsync();
            }
        }
    }
}
