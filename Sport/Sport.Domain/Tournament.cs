namespace Sport.Domain
{
    using Domain.Enums.Tournament;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tournament
    {
        public Tournament()
        {
            this.Players = new HashSet<UserTournament>();
            this.Matches = new HashSet<Match>();
            this.IsStarted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The Tournament Name value cannot exceed 50 characters. ")]
        public string Name { get; set; }

        [Required]
        public int NumberOfPlayers { get; set; }

        public ICollection<UserTournament> Players { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }
             
        public DateTime? EndDate { get; set; }

        [Required]
        public TournamentType Type { get; set; }

        public string Place { get; set; }

        public decimal AmmountOfMoney { get; set; }

        public bool IsStarted { get; set; }

        public ICollection<Match> Matches { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
