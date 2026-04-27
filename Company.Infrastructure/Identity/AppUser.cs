using Microsoft.AspNetCore.Identity;

namespace Company.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}