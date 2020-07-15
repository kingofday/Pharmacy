using Elk.Core;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Elk.EntityFrameworkCore.Tools;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class AppDbContext : ElkDbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(x => x.MobileNumber).HasName("IX_MobileNumber").IsUnique();
            builder.Entity<Drug>().HasIndex(x => x.NameEn).HasName("IX_NameEn");
            builder.Entity<Drug>().HasIndex(x => x.NameFa).HasName("IX_NameFa");
            builder.Entity<OrderDrugStore>().HasIndex(x => new { x.OrderDrugStoreId, x.DrugStoreId }).HasName("IX_OrderDrugStore").IsUnique();
            builder.Entity<DrugPrice>().HasIndex(x => new { x.DrugId, x.UnitId }).HasName("IX_DrugUnit").IsUnique();
            builder.Entity<Tag>().HasIndex(x => x.Title).HasName("IX_Title").IsUnique();
            builder.Entity<TempOrderDetail>().HasIndex(x => x.BasketId).HasName("IX_BasketId");

            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
        }
    }
}