using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chi.SocialNetwork
{
    public class PostCommentDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public int LikeCount { get; set; }
        public UserDTO User { get; set; }
    }
}