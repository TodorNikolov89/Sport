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
            this.Win = initialValue;
            this.Loses = initialValue;
            this.Points = initialValue;
        }
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }          

        public ICollection<UserTournament> Tournaments { get; set; }

        public int Win { get; set; }

        public int Loses { get; set; }
        
        public string Town { get; set; }

        public int Points { get; set; }

        public int Rank { get; set; }

    }
}
