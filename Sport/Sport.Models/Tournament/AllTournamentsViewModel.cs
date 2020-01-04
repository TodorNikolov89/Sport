namespace Sport.ViewModels.Tournament
{
    using Sport.Domain.Enums.Tournament;
    using Sport.ViewModels.User;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllTournamentsViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public ICollection<UserViewModel> Players { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Place { get; set; }

        public TournamentType Type { get; set; }

        public decimal AmmountOfMoney { get; set; }

        public string Description { get; set; }


        public bool IsStarted { get; set; }
    }
}
