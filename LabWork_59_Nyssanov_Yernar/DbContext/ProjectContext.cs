using LabWork_59_Nyssanov_Yernar.Models;
using LabWork_59_Nyssanov_Yernar.Models.PostUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LabWork_59_Nyssanov_Yernar.DbContext;
using Microsoft.EntityFrameworkCore;

public class ProjectContext : IdentityDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

}