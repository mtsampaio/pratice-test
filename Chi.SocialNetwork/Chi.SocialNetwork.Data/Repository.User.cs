using System;
using System.Linq;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        #region Validations

        /// <summary>
        /// Performs all the e-mail validations.
        /// </summary>
        /// <param name="userId">The user to be validated.</param>
        /// <param name="email">The e-mail to be validated.</param>
        /// <returns>True if valid.</returns>
        private bool ValidateUserEmail(int userId, string email)
        {
            var valid = this.entities.Users.Any(p => p.Email == email) == false;
            return valid;
        }

        /// <summary>
        /// Performs all the password validations.
        /// </summary>
        /// <param name="userId">The user to be validated.</param>
        /// <param name="password">The password to be validated.</param>
        /// <returns>True if valid.</returns>
        private bool ValidatePassword(int userId, string password)
        {
            var valid = password.Trim().Length > 0;
            return valid;
        }

        #endregion End of Validations

        #region Selects

        /// <summary>
        /// Gets all users from database.
        /// </summary>
        /// <returns>The Users.</returns>
        public IQueryable<User> GetUsers()
        {
            return entities.Users;
        }

        /// <summary>
        /// Gets a specific user from database by his Id.
        /// </summary>
        /// <param name="id">The desired user Id.</param>
        /// <returns>The user. If not found, returns null.</returns>
        public User GetUserById(int id)
        {
            return this.GetUsers().FirstOrDefault(p => p.Id == id);
        }

        #endregion End of Selects

        #region Inserts

        /// <summary>
        /// Inserts a new user into the Chi Social Network database.
        /// </summary>
        /// <param name="user">The new user.</param>
        /// <returns>The user with the new Id.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the user's e-mail already exists in the database.</exception>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public User InsertUser(User user)
        {
            if (this.ValidateUserEmail(user.Id, user.Email) == false)
            {
                throw new InvalidOperationException(Properties.Resources.DuplicatedUserEmail);
            }

            var insertedUser = this.entities.Users.Add(user);
            this.SaveChanges();
            return insertedUser;
        }

        #endregion End of Inserts

        #region Updates

        /// <summary>
        /// Applies changes made to the user into Chi Social Network database.
        /// <para>This method do not change the use's password. <see cref="Chi.SocialNetwork.Data.Repository.UpdateUser(Chi.SocialNetwork.Data.User, System.Boolean)"/> for more informations.</para>
        /// </summary>
        /// <param name="user">The modified user.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the user validation fails.</exception>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public User UpdateUser(User user)
        {
            return this.UpdateUser(user, false);
        }

        /// <summary>
        /// Applies changes made to the user into Chi Social Network database.
        /// </summary>
        /// <param name="user">The modified user.</param>
        /// <param name="changePassword">Whether or not to change the user password.</param>
        /// <returns>The updated user.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the user validation fails.</exception>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public User UpdateUser(User user, bool changePassword)
        {
            if (this.ValidateUserEmail(user.Id, user.Email) == false)
            {
                throw new InvalidOperationException(Properties.Resources.DuplicatedUserEmail);
            }

			if (changePassword && this.ValidatePassword(user.Id, user.Password) == false)
            {
                throw new InvalidOperationException(Properties.Resources.InvalidUserInformationsMessage);
			}

            // attaches the modified user into its DbSet and sets the entry state to modified.
            this.entities.Users.Attach(user);
            var entry = this.entities.Entry(user);
            entry.State = System.Data.Entity.EntityState.Modified;

            // sets the password property state according to changePassword value.
            entry.Property(p => p.Password).IsModified = changePassword;

            this.SaveChanges();

            return user;
        }

        #endregion End of Updates
    }
}
