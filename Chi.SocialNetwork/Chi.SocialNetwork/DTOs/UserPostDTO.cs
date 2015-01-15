using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chi.SocialNetwork
{
    public class UserPostDTO
    {
        public int Id { get; set; }
        public string Post { get; set; }
        public int Type { get; set; }
        public bool ILiked { get; set; }
        public int LikeCount { get; set; }
        public IEnumerable<PostCommentDTO> Comments { get; set; }
    }
}