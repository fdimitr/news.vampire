using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using News.Vampire.Service.Constants;
using News.Vampire.Service.DataAccess;
using NUnit.Framework;

namespace News.Vampire.Service.Tests.BusinessLogic
{
    [TestFixture]
    public class BaseTests
    {
        protected IConfiguration config;

        public BaseTests()
        {
            config = InitConfiguration();
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Database.EnsureDeleted();
        }
        private IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        protected DbContextOptions<DataContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<DataContext>().UseNpgsql(config.GetValue<string>(ConfigKey.ConnectionString)).Options;
        }

        [OneTimeTearDown]
        public void TotalCleanUp()
        {
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Database.EnsureDeleted();
        }
    }
}
