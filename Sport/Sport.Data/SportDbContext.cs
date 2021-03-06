﻿namespace Sport.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Domain;
    using System;

    public class SportDbContext : IdentityDbContext<User>
    {

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<UserTournament> UserTournament { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Set> Sets { get; set; }

        public SportDbContext(DbContextOptions<SportDbContext> options)
            : base(options)
        {
        }

        public DbSet<TieBreak> TieBreaks { get; set; }

        public DbSet<TieBreakPoint> TieBreakPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            UserTournamentConfiguration(builder);
            MatchConfiguration(builder);
            // ResultConfiguration(builder);
            PointConfiguration(builder);
            GameConfiguration(builder);
            SetConfiguration(builder);
            TieBreakConfiguration(builder);
            TournamentConfiguration(builder);

            base.OnModelCreating(builder);
        }

        private void TournamentConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Tournament>()
                .HasOne(c => c.Creator)
                .WithMany(t => t.CreatedTournaments);
        }

        private void TieBreakConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<TieBreak>()
                .HasMany(tp => tp.TieBreakPoints)
                .WithOne(t => t.TieBreak);
        }

        private void SetConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Set>()
                .HasOne(p => p.Player)
                .WithMany(s => s.Sets);

            builder
                .Entity<Set>()
                .HasOne(t => t.TieBreak)
                .WithOne(s => s.Set);
        }

        private void GameConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Game>()
                .HasOne(s => s.Set)
                .WithMany(g => g.Games);

            builder
                .Entity<Game>()
                .HasOne(p => p.Player)
                .WithMany(g => g.Games);
        }

        private void PointConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Point>()
                .HasOne(g => g.Game)
                .WithMany(p => p.Points);
        }

        //private void ResultConfiguration(ModelBuilder builder)
        //{
        //    builder
        //        .Entity<Result>()
        //        .HasOne(r => r.Match)
        //        .WithOne(m => m.MatchResult)
        //        .HasForeignKey<Match>(fk => fk.MatchResultId);


        //}

        private void MatchConfiguration(ModelBuilder builder)
        {
            builder
                .Entity<Match>()
                .HasOne(t => t.Tournament)
                .WithMany(m => m.Matches);

            //builder
            //    .Entity<Match>()
            //    .HasOne(r => r.MatchResult)
            //    .WithOne(m => m.Match)
            //    .HasForeignKey<Result>(fk => fk.MatchId);

            builder
                .Entity<Match>()
                .HasMany(s => s.Sets)
                .WithOne(m => m.Match);
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
