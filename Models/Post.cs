using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrightIdeas.Models
{
    public class Post
    {
        [Key]
        public long post_id {get;set;}

        [Required]
        [Display(Name="Idea")]
        [MinLength(2)]
        public string idea {get;set;}
        

        public DateTime created_at {get;set;}

        public List<Like> ListLikeUsers {get;set;}

        public long user_id {get;set;}

        public Post()
        {
            created_at = DateTime.Now;
        }

    }
}