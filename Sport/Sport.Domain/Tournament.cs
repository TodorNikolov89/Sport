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
            this.IsStarted = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Too long name. Allowed symbols are 50!")]
        public string Name { get; set; }

        [Required]
        public int NumberOfPlayers { get; set; }

        public ICollection<UserTournament> Users { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public TournamentType Type { get; set; }

        public string Place { get; set; }

        public decimal AmmountOfMoney { get; set; }

        public bool IsStarted { get; set; }

    }
}
