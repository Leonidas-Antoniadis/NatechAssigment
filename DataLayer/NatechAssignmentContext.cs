using Microsoft.EntityFrameworkCore;
using Natech.DataLayer.Models;

namespace Natech.DataLayer
{
    public class NatechAssignmentContext : DbContext
    {
        public NatechAssignmentContext(DbContextOptions<NatechAssignmentContext> options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Batch>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.BatchResult ??= string.Empty;

            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
