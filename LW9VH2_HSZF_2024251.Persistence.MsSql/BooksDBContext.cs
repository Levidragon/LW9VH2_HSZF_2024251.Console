using LW9VH2_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LW9VH2_HSZF_2024251.Persistence.MsSql
{
    public class BooksDBContext: DbContext
    {
        public DbSet<Authors> AuthorsTable { get; set; }
        public DbSet<Books> BooksTable{ get; set; }
        public DbSet<Categories > CategoriesTable{ get; set; }

        public BooksDBContext()
        {
            Database.EnsureCreated();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=booksdb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
