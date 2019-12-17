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
            this.Users = new HashSet<UserTournament>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public ICollection<UserTournament> Users { get; set; }
        
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TournamentType Type { get; set; }

        public string Place { get; set; }

        public decimal AmmountOfMoney { get; set; }

    }
}
