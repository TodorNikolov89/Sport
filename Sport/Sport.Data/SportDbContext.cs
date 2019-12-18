namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Sport.Domain;
    using System;

    public class SportDbContext : IdentityDbContext<User>
    {

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<UserTournament> UserTournament { get; set; }

        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            UserTournamentConfiguration(builder);
            base.OnModelCreating(builder);
        }

        private void UserTournamentConfiguration(ModelBuilder builder)
        {
            builder
             .Entity<UserTournament>()
             .HasKey(pk => new { pk.TournamentId, pk.UserId });
            builder
                .Entity<UserTournament>()
                .HasOne(t => t.Tournament)
                .WithMany(u => u.Users)
                .HasForeignKey(fk => fk.TournamentId);
            builder
              .Entity<UserTournament>()
              .HasOne(t => t.User)
              .WithMany(u => u.Tournaments)
              .HasForeignKey(fk => fk.UserId);

        }
    }
}
