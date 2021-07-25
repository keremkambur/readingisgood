using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReadingIsGood.BusinessLayer.Initializer
{
    // https://github.com/thomaslevesque/AspNetCore.AsyncInitialization/blob/master/src/AspNetCore.AsyncInitialization/AsyncInitializer.cs
    public class AsyncInitializer
    {
        private readonly ILogger<AsyncInitializer> _logger;
        private readonly IEnumerable<IAsyncInitializer> _initializers;

        public AsyncInitializer(ILogger<AsyncInitializer> logger, IEnumerable<IAsyncInitializer> initializers)
        {
            this._logger = logger;
            this._initializers = initializers;
        }

        public async Task InitializeAsync()
        {
            this._logger.LogInformation("Starting async initialization");
            if (this._initializers == null || !this._initializers.Any())
            {
                this._logger.LogInformation("No initializations found.");

                return;
            }

            try
            {
                foreach (var initializer in this._initializers)
                {
                    this._logger.LogInformation("Starting async initialization for {InitializerType}", initializer.GetType());
                    try
                    {
                        await initializer.InitializeAsync().ConfigureAwait(false);
                        this._logger.LogInformation("Async initialization for {InitializerType} completed", initializer.GetType());
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, "Async initialization for {InitializerType} failed", initializer.GetType());
                        throw;
                    }
                }

                this._logger.LogInformation("Async initialization completed");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Async initialization failed");
                throw;
            }
        }
    }
}