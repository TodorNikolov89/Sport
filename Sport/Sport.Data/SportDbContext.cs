namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Sport.Domain;

    public class SportDbContext : IdentityDbContext<User>
    {
        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }
    }
}
