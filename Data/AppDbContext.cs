namespace ExpenseManagerAPI.Data
{
    using ExpenseManagerAPI.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<EventModel> Event { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<EventModel>()
        //        .HasKey(e => e.EventID);  // Explicitly defines EventID as the primary key
        //}

    }

}
