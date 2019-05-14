using Microsoft.EntityFrameworkCore;
using VideoIdxApp.API.Models;

namespace VideoIdxApp.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}

        public DbSet<Pessoa> Pessoas {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Photo> Photos {get; set;}
    }
}