using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrightIdeas.Models;

namespace BrightIdeas.Models
{
    public class UserPost
    {
        public List<User> Users {get;set;}

        public List<Post> Posts {get;set;}
        public User User {get;set;}
        public List<Like> Likes {get;set;}

        [Required]
        [MinLength(2)]
        public string Idea {get;set;}

    }

}