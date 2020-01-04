namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Domain;

    public class SportDbContext : IdentityDbContext<User>
    {

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<UserTournament> UserTournament { get; set; }

        public DbSet<Match> Matches { get; set; }

        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            UserTournamentConfiguration(builder);
            MatchConfiguration(builder);
            base.OnModelCreating(builder);
        }

        private void MatchConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Match>()
                .HasOne(t => t.Tournament)
                .WithMany(m => m.Matches);
        }

        private void UserTournamentConfiguration(ModelBuilder builder)
        {
            builder
             .Entity<UserTournament>()
             .HasKey(pk => new { pk.TournamentId, pk.UserId });

            builder
                .Entity<UserTournament>()
                .HasOne(t => t.Tournament)
                .WithMany(u => u.Players)
                .HasForeignKey(fk => fk.TournamentId);
            builder
              .Entity<UserTournament>()
              .HasOne(t => t.User)
              .WithMany(u => u.Tournaments)
              .HasForeignKey(fk => fk.UserId);

        }
    }
}
