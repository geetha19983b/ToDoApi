using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Todo.Repository
{
    public class AuthorContext : DbContext
    {
        public AuthorContext(DbContextOptions<AuthorContext> options)
          : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }

    }
}
