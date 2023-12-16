using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using News.Vampire.Service.BusinessLogic.Interfaces;

namespace News.Vampire.Service.Logger
{
    [ProviderAlias("Database")]  
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;

        public DbLoggerProvider(IOptions<DbLoggerOptions> options)
        {
            Options = options.Value; // Stores all the options.
        }

        /// <summary>  
        /// Creates a new instance of the db logger.  
        /// </summary>  
        /// <param name="categoryName"></param>  
        /// <returns></returns>  
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
