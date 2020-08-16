using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Pharmacy.DataAccess.Ef
{
    public sealed class AppUnitOfWork : IElkUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private readonly IServiceProvider _serviceProvider;

        public AppUnitOfWork(IServiceProvider serviceProvider, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _serviceProvider = serviceProvider;
        }


        #region Base
        public IUserRepo UserRepo => _serviceProvider.GetService<IUserRepo>();
        public ITagRepo TagRepo => _serviceProvider.GetService<ITagRepo>();
        public IGenericRepo<UserAddress> UserAddressRepo => _serviceProvider.GetService<IGenericRepo<UserAddress>>();
        public IGenericRepo<DrugStoreAddress> DrugStoreAddressRepo => _serviceProvider.GetService<IGenericRepo<DrugStoreAddress>>();
        public INotificationRepo NotificationRepo => _serviceProvider.GetService<INotificationRepo>();
        public IGenericRepo<DrugStore> DrugStoreRepo => _serviceProvider.GetService<IGenericRepo<DrugStore>>();
        #endregion

        #region Order
        public IGenericRepo<Order> OrderRepo => _serviceProvider.GetService<IGenericRepo<Order>>();
        public IGenericRepo<OrderItem> OrderDetailRepo => _serviceProvider.GetService<IGenericRepo<OrderItem>>();
        public IGenericRepo<Prescription> PrescriptionRepo => _serviceProvider.GetService<IGenericRepo<Prescription>>();
        public IPaymentRepo PaymentRepo => _serviceProvider.GetService<IPaymentRepo>();
        public ITempBasketItemRepo TempBasketItemRepo => _serviceProvider.GetService<ITempBasketItemRepo>();
        #endregion

        #region Drug
        public IDrugRepo DrugRepo => _serviceProvider.GetService<IDrugRepo>();
        public IGenericRepo<DrugTag> DrugTagRepo => _serviceProvider.GetService<IGenericRepo<DrugTag>>();
        public IGenericRepo<DrugAsset> DrugAssetRepo => _serviceProvider.GetService<IGenericRepo<DrugAsset>>();
        #endregion


        
        public ChangeTracker ChangeTracker { get => _appDbContext.ChangeTracker; }
        public DatabaseFacade Database { get => _appDbContext.Database; }

        public SaveChangeResult ElkSaveChanges()
            => _appDbContext.ElkSaveChanges();

        public Task<SaveChangeResult> ElkSaveChangesAsync(CancellationToken cancellationToken = default)
            => _appDbContext.ElkSaveChangesAsync(cancellationToken);

        public SaveChangeResult ElkSaveChangesWithValidation()
            => _appDbContext.ElkSaveChangesWithValidation();

        public Task<SaveChangeResult> ElkSaveChangesWithValidationAsync(CancellationToken cancellationToken = default)
            => _appDbContext.ElkSaveChangesWithValidationAsync(cancellationToken);

        public int SaveChanges()
            => _appDbContext.SaveChanges();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _appDbContext.SaveChangesAsync(cancellationToken);

        public Task<int> SaveChangesAsync(bool AcceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => _appDbContext.SaveChangesAsync(AcceptAllChangesOnSuccess, cancellationToken);

        public void Dispose() => _appDbContext.Dispose();
    }
}