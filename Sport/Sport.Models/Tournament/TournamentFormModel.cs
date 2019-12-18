namespace Sport.ViewModels.Tournament
{
    using Domain.Enums.Tournament;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class TournamentFormModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int NumberOfPlayers { get; set; }

        [Display(Name = "Start date")]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Place")]
        public string Place { get; set; }

        [Required]
        public TournamentType Type { get; set; }

        [Display(Name = "Amount of money for Charity/Prize Money")]
        public decimal AmmountOfMoney { get; set; }


        public string Description { get; set; }


    }
}
