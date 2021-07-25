using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ReadingIsGood.BusinessLayer.Contracts;
using ReadingIsGood.BusinessLayer.Initializer;

namespace ReadingIsGood.BusinessLayer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers necessary services for async initialization support.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddAsyncInitialization(this IServiceCollection services)
        {
            services.TryAddTransient<AsyncInitializer>();
            return services;
        }

        /// <summary>
        /// Adds an async initializer of the specified type.
        /// </summary>
        /// <typeparam name="TInitializer">The type of the async initializer to add.</typeparam>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddAsyncInitializer<TInitializer>(this IServiceCollection services)
            where TInitializer : class, IAsyncInitializer
        {
            return services
                .AddAsyncInitialization()
                .AddTransient<IAsyncInitializer, TInitializer>();
        }
    }
}
