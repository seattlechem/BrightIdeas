using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrightIdeas.Models
{
    public class Like
    {
        [Key]
        public long like_id {get;set;}

        public DateTime created_at {get;set;}
    
        public long user_id {get;set;}
        public long post_id {get;set;}
        
        public User LikeUser {get;set;}
        public Post LikePost {get;set;}

        public Like()
        {
            created_at = DateTime.Now;

        }


    }
}