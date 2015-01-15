//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chi.SocialNetwork.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPost
    {
        public UserPost()
        {
            this.UserPostComments = new HashSet<UserPostComment>();
            this.UserPostLikes = new HashSet<UserPostLike>();
        }
    
        public int Id { get; set; }
        public string PostContent { get; set; }
        public int ContentType { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public int User_Id { get; set; }
    
        public virtual ICollection<UserPostComment> UserPostComments { get; set; }
        public virtual ICollection<UserPostLike> UserPostLikes { get; set; }
        public virtual User User { get; set; }
    }
}
