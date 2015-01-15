using System.ComponentModel.DataAnnotations;

namespace Chi.SocialNetwork.Data
{
    [MetadataType(typeof(UserMetada))]
    public partial class User
    {
        internal sealed class UserMetada
        {
            private UserMetada() { }
        }
    }
}
