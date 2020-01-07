﻿
namespace Sport.ViewModels.User
{
    using System;

    public class UserDrawViewModel
    {

        public string Id { get; set; }

        public DateTime? DateOfBirth { get; set; }
       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public int Win { get; set; }

        public int Loses { get; set; }

        public string Town { get; set; }

        public int Points { get; set; }

        public int Rank { get; set; }
    }
}
