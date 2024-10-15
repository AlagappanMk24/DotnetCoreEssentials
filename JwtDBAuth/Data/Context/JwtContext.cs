using JwtAuthDB.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JwtAuthDB.Data.Context
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
