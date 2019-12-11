using Microsoft.AspNetCore.Identity;

namespace Sport.Domain
{
    public class User : IdentityUser
    {
        public string DataOFBirth { get; set; }
    }
}
