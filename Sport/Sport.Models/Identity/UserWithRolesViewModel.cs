using System;
using System.Collections.Generic;
using System.Text;

namespace Sport.ViewModels.Identity
{
    public class UserWithRolesViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
