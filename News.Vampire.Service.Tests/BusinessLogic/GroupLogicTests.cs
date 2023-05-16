using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.Constants;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using NUnit.Framework;

namespace News.Vampire.Service.Tests.BusinessLogic
{
    [TestFixture]
    public class GroupLogicTests
    {
        private IConfiguration config;

        public GroupLogicTests()
        {
            config = InitConfiguration();
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Database.EnsureDeleted();
        }

        public DbContextOptions<DataContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<DataContext>().UseNpgsql(config.GetValue<string>(ConfigKey.ConnectionString)).Options;

        }

        [TearDown]
        public void CleanUp()
        {
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Groups.RemoveRange(dbTestContext.Groups);
        }

        [OneTimeTearDown]
        public void TotalCleanUp()
        {
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Database.EnsureDeleted();
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

        [Test]
        public async Task GetAll()
        {
            // Arrange
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Groups.AddRange(
                new Group
                {
                    Name = "test1"
                }, new Group
                {
                    Name = "test2"
                });
            dbTestContext.SaveChanges();

            var mockDbFactory = new Mock<IDbContextFactory<DataContext>>();
            mockDbFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbTestContext);

            GroupLogic groupLogic = new GroupLogic(mockDbFactory.Object);

            // Act
            var actualGroups = await groupLogic.GetAllAsync();

            //Assert
            Assert.That(actualGroups, Is.Not.Null.Or.Empty);
            Assert.That(actualGroups.Count, Is.EqualTo(2));
        }

    }
}
