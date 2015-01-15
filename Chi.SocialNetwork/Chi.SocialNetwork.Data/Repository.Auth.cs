using System.Data.Entity;
using System.Threading.Tasks;

namespace Chi.SocialNetwork.Data
{
    public partial class Repository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<User> GetUserAsync(int userId)
        {
            return this.entities.Users.FirstOrDefaultAsync(p => p.Id == userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<User> CreateUserAsync(User user)
        {
            return this.InsertUserAsync(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<User> LoginAsync(string email, string password)
        {
            return this.entities.Users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
        }
    }
}
