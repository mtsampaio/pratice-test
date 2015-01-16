using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        /// <summary>
        /// Gets all the files of a post from Chi Social Network database.
        /// </summary>
        /// <param name="postId">The post id.</param>
        /// <returns>The files of the post.</returns>
        public IQueryable<UserPostFile> GetUserPostFiles(int postId)
        {
            return this.entities.UserPostFiles.Where(p => p.UserPostId == postId);
        }

        /// <summary>
        /// Gets a specific file of a post from Chi Social Network database.
        /// </summary>
        /// <param name="postId">The file id.</param>
        /// <returns>The file of the post.</returns>
        public UserPostFile GetUserPostFile(int fileId)
        {
            return this.entities.UserPostFiles.FirstOrDefault(p => p.Id == fileId);
        }

        /// <summary>
        /// Inserts a single file of a post into Chi Social Network database.
        /// </summary>
        /// <param name="postFile">The file to be inserted.</param>
        /// <returns>The inserted file.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when postFile is null.</exception>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public UserPostFile InsertUserPostFile(UserPostFile postFile)
        {
            if (postFile == null)
            {
                throw new ArgumentNullException("postFile");
            }

            postFile = this.entities.UserPostFiles.Add(postFile);
            this.SaveChanges();
            return postFile;
        }

        /// <summary>
        /// Inserts a list of files of a post into Chi Social Network database.
        /// </summary>
        /// <param name="postFiles">The files to be inserted.</param>
        /// <returns>The inserted files.</returns>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public IQueryable<UserPostFile> InsertUserPostFiles(IEnumerable<UserPostFile> postFiles)
        {
            if (postFiles == null)
            {
                throw new ArgumentNullException("postFile");
            }

            postFiles = this.entities.UserPostFiles.AddRange(postFiles);
            this.SaveChanges();
            return postFiles.AsQueryable();
        }
    }
}
