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
            this.Players = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public ICollection<User> Players { get; set; }

        public int SiginPlayers => this.Players.Count;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TournamentType Type { get; set; }

        public decimal AmmountOfMoney { get; set; }

    }
}
