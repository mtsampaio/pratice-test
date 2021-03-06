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
    
    public partial class UserPostComment
    {
        public UserPostComment()
        {
            this.UserPostCommentLikes = new HashSet<UserPostCommentLike>();
        }
    
        public int Id { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public int User_Id { get; set; }
        public int UserPost_Id { get; set; }
    
        public virtual User User { get; set; }
        public virtual UserPost UserPost { get; set; }
        public virtual ICollection<UserPostCommentLike> UserPostCommentLikes { get; set; }
    }
}
