using Microsoft.AspNetCore.Identity;

namespace ElektronikWebUI.Identity
{
    public class Customer:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePhoto { get; set; }
        public string Password { get; set; }
       

    }
}
