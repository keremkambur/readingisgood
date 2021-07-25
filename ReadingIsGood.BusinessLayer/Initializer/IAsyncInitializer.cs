using System.Threading.Tasks;

namespace ReadingIsGood.BusinessLayer.Initializer
{
    // https://github.com/thomaslevesque/AspNetCore.AsyncInitialization/blob/master/src/AspNetCore.AsyncInitialization/IAsyncInitializer.cs
    /// <summary>
    /// Represents a type that performs async initialization.
    /// </summary>
    public interface IAsyncInitializer
    {
        /// <summary>
        /// Performs async initialization.
        /// </summary>
        /// <returns>A task that represents the initialization completion.</returns>
        Task InitializeAsync();
    }
}