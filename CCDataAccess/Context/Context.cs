using Microsoft.EntityFrameworkCore;

namespace CCDataAccess.Context
{
    public class AddiDbContext : DbContext
    {
        public AddiDbContext(DbContextOptions<AddiDbContext> options) : base(options)
        {

        }
        
    }
}
