using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class NatechAssignmentContext : DbContext
    {
        public NatechAssignmentContext(DbContextOptions<NatechAssignmentContext> options) : base(options)
        {
        }

        public DbSet<Geolocation> Geolocation { get; set; }
    }
}
