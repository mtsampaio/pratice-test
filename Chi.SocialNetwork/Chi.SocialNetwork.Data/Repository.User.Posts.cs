using System;
using System.Linq;

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

        public UserPost InsertUserPost(UserPost post)
        {
            throw new NotImplementedException();
        }

        public UserPost UpdateUserPost(int userId, UserPost post)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserPost(int userPostId)
        {
            throw new NotImplementedException();
        }
    }
}
