using Elk.Core;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Pharmacy.Delivery.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pharmacy.Delivery.DependencyResolver
{
    public static class DeliveryDiExtension
    {
        public static IServiceCollection AddTransient(this IServiceCollection serviceCollection, IConfiguration _configuration)
        {
            //serviceCollection.AddTransient<IUserRepo, UserRepo>();

            return serviceCollection;
        }

        public static IServiceCollection AddScoped(this IServiceCollection services, IConfiguration _configuration)
        {
            //services.AddContext<DeliveryDbContext>(_configuration.GetConnectionString("DeliveryDbContext"));

            //services.AddScoped<DeliveryUnitOfWork, DeliveryUnitOfWork>();

            services.AddScoped(typeof(IGenericRepo<>), typeof(EfGenericRepo<>));


            #region Delivery

            //services.AddScoped<IEventMapperRepo, EventMapperRepo>();

            services.AddScoped<IDeliveryService, DeliveryService>();

            #endregion

            return services;
        }

        public static IServiceCollection AddSingleton(this IServiceCollection services, IConfiguration _configuration)
        {
            //services.AddSingleton<IMemoryCacheProvider, MemoryCacheProvider>();
            
            return services;
        }

        public static IServiceCollection AddContext<TDbContext>(this IServiceCollection serviceCollection, string conectionString) where TDbContext : DbContext
        {
            serviceCollection.AddDbContext<TDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(conectionString,
                            sqlServerOption =>
                            {
                                sqlServerOption.MaxBatchSize(1000);
                                sqlServerOption.CommandTimeout(null);
                                sqlServerOption.UseRelationalNulls(false);
                                //sqlServerOption.EnableRetryOnFailure();
                                //sqlServerOption.UseRowNumberForPaging(false);
                            });
                //.AddInterceptors(new DbContextInterceptors());
            });

            return serviceCollection;
        }
    }
}
