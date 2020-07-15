using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Notifier.DataAccess.Ef
{
    public class NotifierDbContext : ElkDbContext
    {
        public NotifierDbContext() { }

        public NotifierDbContext(DbContextOptions<NotifierDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Page>().HasIndex(x => x.Username).HasName("IX_Username").IsUnique();

            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<INotifierEntity>(typeof(Notification).Assembly);
        }
    }
}
