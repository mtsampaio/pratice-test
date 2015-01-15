using Chi.SocialNetwork.Data;
using System;
using System.Linq;
using System.Security.Claims;

namespace Chi.SocialNetwork
{
    /// <summary>
    /// A class to help with user routines.
    /// </summary>
    public class CurrentUserHelper
    {
        /// <summary>
        /// Gets the user entity of the current logged in identity at the repository.
        /// </summary>
        /// <param name="identity">The identity object where the user informations are stored.</param>
        /// <returns>The current user entity</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when identity is null.</exception>
        public static User GetCurrentUser(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            using (var repository = new Repository())
            {
                var loggedUserId = identity.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

                if (loggedUserId != null && string.IsNullOrEmpty(loggedUserId.Value) == false)
                {
                    return repository.GetUserById(Convert.ToInt32(loggedUserId.Value));
                }

                return null;
            }
        }
    }
}