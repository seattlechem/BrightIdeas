using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrightIdeas.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BrightIdeas.Controllers
{
    public class HomeController : Controller
    {
        private MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
        
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(newUser user)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            //check uniquness of username
            if(_context.users.Where(u => u.email == user.email)
                             .ToList()
                             .Count() > 0)
            {
                ModelState.AddModelError("userName", "Username already exists");
            }

            else if(ModelState.IsValid)
            {
                User toCreate = new User()
                {
                    name = user.name,
                    alias = user.alias,
                    email = user.email,
                    password = hasher.HashPassword(user, user.password)
                };

                _context.users.Add(toCreate);
                _context.SaveChanges();

                

                return RedirectToAction("Index", "Home");
            }
           
            return View("index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            //check if username exists
            if(_context.users.Where(u => u.email == user.logEmail)
                             .ToList()
                             .Count() == 0)  
            {
                ModelState.AddModelError("logUserName", "Email does not exist");
            }
            else if(ModelState.IsValid)
            {
                //check if password is correct
                User loggedUser = _context.users.SingleOrDefault(u =>u.email == user.logEmail);
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(loggedUser, loggedUser.password, user.logPassword)) 
                {
                    HttpContext.Session.SetInt32("id", (int)loggedUser.user_id);
                    return RedirectToAction("Ideas", "Post");
                }
                else
                {
                    ModelState.AddModelError("logPassword", "Email/Password is incorrect");
                }
            }
           
            return View("index");
        }
    }
}
