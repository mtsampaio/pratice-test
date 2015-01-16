using Chi.SocialNetwork.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Chi.SocialNetwork.Controllers
{
    public class UserController : ApiController
    {
        private Repository repository = new Repository();

        // GET: api/User
        public DataSourceResult GetUsers([ModelBinder(typeof(WebApiDataSourceRequestModelBinder))] DataSourceRequest request)
        {
            return repository.GetUsers()
                .Select(p => new UserDTO
                {
                    Id = p.Id,
                    Name = p.Name + " " + p.LastName,
                    Portrait = ""
                })
                .ToDataSourceResult(request);
        }
    }
}