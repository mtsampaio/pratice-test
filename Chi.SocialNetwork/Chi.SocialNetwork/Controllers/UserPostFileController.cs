using Chi.SocialNetwork.Data;
using System.Web.Http;
using System.Web.Http.Description;

namespace Chi.SocialNetwork.Controllers
{
    public class UserPostFileController : ApiController
    {
        private Repository repository = new Repository();

        // POST: api/UserPostFile
        [ResponseType(typeof(PostFileDTO))]
        public IHttpActionResult PostUserPostFile(UserPostFile userPostFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userPostFile = repository.InsertUserPostFile(userPostFile);

            return CreatedAtRoute("DefaultApi", new { id = userPostFile.Id }, new PostFileDTO {
                Id = userPostFile.Id,
                PostId = userPostFile.UserPostId,
                FileName = userPostFile.FileName,
                Type  = userPostFile.Type
            });
        }
    }
}