namespace Sport.ViewModels.User
{
    using System;
    using Sport.Domain;
    public class UserViewModel
    {
        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

    }
}
