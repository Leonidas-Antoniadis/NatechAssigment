using Microsoft.EntityFrameworkCore;
using Natech.DataLayer.Models;

namespace DataLayer
{
    public class NatechAssignmentContext : DbContext
    {
        public NatechAssignmentContext(DbContextOptions<NatechAssignmentContext> options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
    }
}
