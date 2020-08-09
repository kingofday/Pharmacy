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
            builder.Entity<Tag>().HasIndex(x => x.Name).HasName("IX_Title").IsUnique();
            builder.Entity<MenuSPModel>().HasNoKey().ToView(null);
            builder.OverrideDeleteBehavior();
            builder.RegisterAllEntities<IEntity>(typeof(User).Assembly);
        }
        public DbSet<MenuSPModel> MenuSPModel { get; set; }
    }
}