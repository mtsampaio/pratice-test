using Chi.SocialNetwork.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;
using System.Web.Http;

namespace Chi.SocialNetwork.Controllers
{
    public class UserController : ApiController
    {
        private Repository repository = new Repository();

        // GET: api/User
        public IQueryable<User> GetUsers()
        {
            return repository.GetUsers();
        }

        // GET: api/User
        public DataSourceResult GetUsersKendo([System.Web.Http.ModelBinding.ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return repository.GetUsers().ToDataSourceResult(request);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}