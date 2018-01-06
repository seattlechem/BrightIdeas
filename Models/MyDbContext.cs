using Microsoft.EntityFrameworkCore;
using BrightIdeas.Models;

namespace BrightIdeas.Models
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options){}
        public DbSet<User> users {get; set;}
        public DbSet<Post> posts {get; set;}
        public DbSet<Like> likes {get; set;}        
    }
}