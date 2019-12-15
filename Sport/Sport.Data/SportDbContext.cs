namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Sport.Domain;
    using System;

    public class SportDbContext : IdentityDbContext<User>
    {

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Player> Players { get; set; }

        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            PlayerConfiguration(builder);
            UserConfiguration(builder);
            TournamentConfiguration(builder);
            base.OnModelCreating(builder);
        }

        private void TournamentConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Tournament>()
                .HasMany(p => p.Players)
                .WithOne(t => t.Tournament)
                .IsRequired();
        }

        private void UserConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasOne(t => t.Tournament)
                .WithMany(u => u.Players);
        }

        private void PlayerConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Player>()
                .HasOne(u => u.User)
                .WithOne(p => p.Player)
                .HasForeignKey<Player>(fk => fk.UserId);
        }
    }
}
