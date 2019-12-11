namespace Sport.Domain
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class User : IdentityUser
    {
        public DateTime? DateOfBirth { get; set; }
    }
}
