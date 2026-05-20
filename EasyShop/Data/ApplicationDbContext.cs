using Microsoft.EntityFrameworkCore;

namespace EasyShop.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected ApplicationDbContext()
    {
    }
}
