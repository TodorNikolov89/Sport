namespace Sport.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        private static readonly int initialValue = 0;

        public User()
        {
            this.Tournaments = new HashSet<UserTournament>();
            this.CreatedTournaments = new HashSet<Tournament>();
            this.Win = initialValue;
            this.Loses = initialValue;
            this.Points = initialValue;
        }
        public DateTime? DateOfBirth { get; set; }

        [Required(AllowEmptyStrings =false,ErrorMessage ="Please eneter first name.")]
        [StringLength(50, ErrorMessage = "The FirstName value cannot exceed 25 characters. ")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please eneter last name.")]
        [StringLength(50, ErrorMessage = "The LastName value cannot exceed 50 characters. ")]
        public string LastName { get; set; }

        public ICollection<UserTournament> Tournaments { get; set; }

        public int Win { get; set; }

        public int Loses { get; set; }

        public string Town { get; set; }

        public int Points { get; set; }

        public int Rank { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<Set> Sets { get; set; }

        public ICollection<Tournament> CreatedTournaments { get; set; }

    }
}
