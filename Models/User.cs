using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrightIdeas.Models
{
    public class User
    {
        [Key]
        public long user_id {get;set;}

        [Required]
        [Display(Name="Name")]
        [MinLength(2)]
        public string name {get;set;}
        
        [Required]
        [Display(Name="Alias")]
        public string alias {get;set;}

        [Required]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string email {get;set;}

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string password {get;set;}


        public DateTime created_at {get;set;}

        public DateTime updated_at {get;set;}
        
        
        public List<Like> ListLikePosts {get;set;}

        public User()
        {
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

        }
        
        
    }

    public class newUser: User
    {
        [Required]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage="Password does not match")]
        public string confirm {get;set;}
        
    }

    public class LoginUser
    {
        [Required]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string logEmail {get;set;}

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string logPassword {get;set;}

    }
}