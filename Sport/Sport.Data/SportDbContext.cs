namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Sport.Domain;

    public class SportDbContext : IdentityDbContext<User>
    {

        public DbSet<Tournament> Tournaments { get; set; }

        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }
    }
}
