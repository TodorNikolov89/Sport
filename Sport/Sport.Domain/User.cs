namespace Sport.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class User : IdentityUser
    {
        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
