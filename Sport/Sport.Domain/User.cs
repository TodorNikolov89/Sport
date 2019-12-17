namespace Sport.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System;

    public class User : IdentityUser
    {
        public User()
        {
            this.Tournaments = new HashSet<UserTournament>();
        }
        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }          

        public ICollection<UserTournament> Tournaments { get; set; }



    }
}
