using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        /// <summary>
        /// Gets all the user posts from Chi Social Network database.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>All the user posts.</returns>
        public IQueryable<UserPost> GetUserPosts(int userId)
        {
            return this.entities.UserPosts.Where(p => p.User.Id == userId);
        }

        /// <summary>
        /// Inserts a new post asynchronously into Chi Social Network.
        /// </summary>
        /// <param name="post">The post to be inserted.</param>
        /// <returns>The inserted post.</returns>
        public async Task<UserPost> InsertUserPostAsync(UserPost post)
        {
            post = this.entities.UserPosts.Add(post);
            await this.SaveChangesAsync();
            return post;
        }
    }
}
