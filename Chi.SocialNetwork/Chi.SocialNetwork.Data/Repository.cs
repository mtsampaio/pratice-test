using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace Chi.SocialNetwork.Data
{
    /// <summary>
    /// Provides methods to access Chi Social Network database entities.
    /// </summary>
    public partial class Repository
    {
        SocialNetworkEntities entities;

        /// <summary>
        /// Initialize a new instance of Chi.SocialNetwork.Data.Repository class.
        /// </summary>
        public Repository()
        {
            entities = new SocialNetworkEntities();
        }

        /// <summary>
        /// Saves all modifications made within the Chi Social Network database.
        /// </summary>
        /// <returns>Number of affected records.</returns>
        /// <exception cref="Chi.SocialNetwork.Data.RepositoryException">Thrown when database actions fail.</exception>
        public int SaveChanges()
        {
            try
            {
                return this.entities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new RepositoryException(Properties.Resources.DbUpdateConcurrencyExceptionMessage);
            }
            catch (DbUpdateException)
            {
                throw new RepositoryException(Properties.Resources.DbUpdateExceptionGenericMessage);
            }
            catch (DbEntityValidationException)
            {
                throw new RepositoryException(Properties.Resources.DbEntityValidationExceptionMessage);
            }
            catch (ObjectDisposedException)
            {
                throw new RepositoryException(Properties.Resources.ConnectionDisposedMessage);
            }
            catch (Exception)
            {
                throw new RepositoryException(Properties.Resources.UnknownErrorMessage);
            }
        }
    }
}
