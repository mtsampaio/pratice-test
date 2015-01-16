using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chi.SocialNetwork
{
    public class PostFileDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
    }
}