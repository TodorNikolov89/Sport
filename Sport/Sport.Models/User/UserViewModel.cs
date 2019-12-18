namespace Sport.ViewModels.User
{
    using System;
    using Sport.Domain;

    public class UserViewModel
    {
        public string Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

    }
}
