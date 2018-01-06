using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BrightIdeas.Controllers
{
    public class PostController : Controller
    {
        private MyDbContext _context;
        public PostController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("ideas")]
        public IActionResult Ideas()
        {
            
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ID = loggedUserId;
            } 


            UserPost newUserPost = new UserPost()
            {  
                Users = _context.users.ToList(),
                User = _context.users.Include(u => u.ListLikePosts)
                                    .SingleOrDefault(u => u.user_id == loggedUserId),
                
                // Posts = _context.posts.Include(p => p.ListLikeUsers)
                //                     .OrderByDescending(o =>o.ListLikeUsers).ToList(),
                Posts = _context.posts.ToList(),
                Likes = _context.likes.ToList(),
                
            };
                           
            return View(newUserPost); 

                    
        }
             
        

        [HttpPost("add")]
        public IActionResult Add(UserPost userPost)
        {
        
            if(ModelState.IsValid)
            {
                Post postToCreate = new Post()
                {
                    idea = userPost.Idea,

                    user_id = (int)HttpContext.Session.GetInt32("id")
       
                };

                _context.posts.Add(postToCreate);
                _context.SaveChanges();

                return RedirectToAction("Ideas", "Post");
            }
            
           
            return RedirectToAction("Ideas");
        }


        [HttpGet("ideas/{post_id}")]
        public IActionResult Display(int post_id)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
            
            //store post_id into session
            HttpContext.Session.SetInt32("post_id", post_id);

            UserPost newUserPost = new UserPost()
            {
                Users = _context.users.ToList(),
                User = _context.users
                                    .Include(u => u.ListLikePosts)
                                    .SingleOrDefault(u => u.user_id == loggedUserId),
                Posts = _context.posts.Where(a => a.post_id == post_id)
                                    .OrderByDescending(o =>o.ListLikeUsers)
                                    .ToList(),
                Likes = _context.likes.Where(b => b.post_id == post_id).ToList()
                


            };
            
            return View(newUserPost);
        }


        [HttpGet("users/{user_id}")]
        public IActionResult Profile(int user_id)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
            
            //store user_id into session
            HttpContext.Session.SetInt32("user_id", user_id);

            UserPost newUserPost = new UserPost()
            {
                Users = _context.users.ToList(),
                User = _context.users
                                    .Include(u => u.ListLikePosts)
                                    .SingleOrDefault(u => u.user_id == user_id),
                Posts = _context.posts.ToList(),
                
                Likes = _context.likes.Where(b => b.user_id == user_id).ToList()
                


            };
            
            return View(newUserPost);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("likes/{post_id}")]
        public IActionResult Likes(int post_id)
        {
            
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

           int likeUserId = (int)HttpContext.Session.GetInt32("id");
           int likePostId = (int)post_id;
           
            //create a new like object
            Like newLike = new Like()
            {
                user_id = likeUserId,
                post_id = likePostId
            };

            _context.likes.Add(newLike);
            _context.SaveChanges();
    
            return RedirectToAction("Ideas", "Post");
        
        }

        [HttpGet("likestatus/{post_id}")]
        public IActionResult LikeStatus(int post_id)
        {
            
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }
           
            UserPost newUserPost = new UserPost()
            {
                Users = _context.users.ToList(),
                User = _context.users
                                    .Include(u => u.ListLikePosts)
                                    .SingleOrDefault(u => u.user_id == loggedUserId),
                Posts = _context.posts.Where(p => p.post_id == post_id).ToList(),
                
                Likes = _context.likes.Where(p => p.post_id == post_id).ToList()
                


            };
            
            return View(newUserPost);
        
        }



        [HttpGet("delete/{post_id}")]
        public IActionResult Delete(int post_id)
        {
            int? loggedUserId = HttpContext.Session.GetInt32("id");
            
            if(loggedUserId == null)
            {
                ViewBag.Status = "Please log in first.";
                return RedirectToAction("Index", "Home");
            }

            List<Like> likeToRemove = _context.likes.Where(a => a.post_id == post_id).ToList();
            
            if(likeToRemove != null)
            {
               foreach(var remove in likeToRemove)
                {
                    _context.likes.Remove(remove);
                    _context.SaveChanges();
                }
            }

            List<Post> postToRemove = _context.posts.Where(a => a.post_id == post_id).ToList();

            if (postToRemove != null)
            {
                foreach(var remove in postToRemove)
                {
                    _context.posts.Remove(remove);
                    _context.SaveChanges();
                }

            }


            return RedirectToAction("Ideas", "Post");
        }
    }

}