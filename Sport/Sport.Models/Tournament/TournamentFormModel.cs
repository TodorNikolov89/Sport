﻿namespace Sport.ViewModels.Tournament
{
    using Sport.Domain;
    using Sport.Domain.Enums.Tournament;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TournamentFormModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Place { get; set; }
        public TournamentType Type { get; set; }

        public decimal AmmountOfMoney { get; set; }

        public string Description { get; set; }


    }
}
