using Microsoft.EntityFrameworkCore;
using Lab2.Models;

namespace Lab2.Data
{
    public class Lab2Context : DbContext
    {
        public Lab2Context(DbContextOptions<Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Publisher> Publisher { get; set; } = default!;
        public DbSet<Author> Author { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { ID = 1, PublisherName = "Humanitas" },
                new Publisher { ID = 2, PublisherName = "Nemira" },
                new Publisher { ID = 3, PublisherName = "Arthur" }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { ID = 1, FirstName = "Marin", LastName = "Preda" },
                new Author { ID = 2, FirstName = "Frank", LastName = "Herbert" }
            );



            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    ID = 1,
                    Title = "Moromeții",
                 
                    Price = 39.90m,
                    PublishingDate = new DateTime(2020, 5, 1),
                    PublisherID = 1,
                    AuthorID = 1
                },
                new Book
                {
                    ID = 2,
                    Title = "Dune",
                    
                    Price = 59.90m,
                    PublishingDate = new DateTime(2019, 11, 15),
                    PublisherID = 2,
                    AuthorID = 2
                }
            );
        }
    }
}
