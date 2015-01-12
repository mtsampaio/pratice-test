using System;

namespace Chi.SocialNetwork.Data
{
    /// <summary>
    /// Represents erros that occur during in Chi Social Network Repository.
    /// </summary>
    [Serializable]
    public class RepositoryException : Exception
    {
        /// <summary>
        /// Initialize a new instance of Chi.SocialNetwork.Data.RepositoryException class.
        /// </summary>
        public RepositoryException() { }
        /// <summary>
        /// Initialize a new instance of Chi.SocialNetwork.Data.RepositoryException class with a specific error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RepositoryException(string message) : base(message) { }
    }
}
