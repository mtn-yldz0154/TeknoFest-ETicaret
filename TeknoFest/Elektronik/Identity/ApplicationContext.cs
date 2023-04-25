using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElektronikWebUI.Identity
{
    public class ApplicationContext:IdentityDbContext<Customer>
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {


        }
    }
}
