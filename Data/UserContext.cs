using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data;

public class UserContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}
