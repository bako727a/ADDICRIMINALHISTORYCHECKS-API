using Microsoft.EntityFrameworkCore;

namespace TANFDataAccess.Context
{
    public class AddiDbContext : DbContext
    {
        public AddiDbContext(DbContextOptions<AddiDbContext> options) : base(options)
        {

        }

    }
}
