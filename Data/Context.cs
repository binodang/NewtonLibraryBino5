using NewtonLibraryBino5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewtonLibraryBino5.Data
{
    internal class Context : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoanCard> LoanCards { get; set; }

        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost; Database=NewtonLibraryBino; Trusted_Connection=True; Trust Server Certificate =Yes; User Id=NewtonLibraryBino; password=NewtonLibrary");
        }
    }
}
