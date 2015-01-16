using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        /// <summary>
        /// Insert a new comment asynchronously to the specified post.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the comment parameter is null.</exception>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public async Task<UserPostComment> InsertUserPostCommentAsync(UserPostComment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            this.entities.UserPostComments.Add(comment);
            await this.SaveChangesAsync();
            return comment;
        }
    }
}
