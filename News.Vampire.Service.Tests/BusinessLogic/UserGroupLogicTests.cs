using Microsoft.EntityFrameworkCore;
using Moq;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using NUnit.Framework;

namespace News.Vampire.Service.Tests.BusinessLogic
{
    public class UserGroupLogicTests : BaseTests
    {
        [TearDown]
        public void CleanUp()
        {
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.UserGroups.RemoveRange(dbTestContext.UserGroups);
            dbTestContext.Subscriptions.RemoveRange(dbTestContext.Subscriptions);
        }

        [Test]
        public async Task GetAllByUserAsyncTest()
        {
            // Arrange
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);

            dbTestContext.Users.AddRange(
                new User
                {
                    Id = 1,
                }, new User
                {
                    Id = 2,
                });

            dbTestContext.Sources.AddRange(
                new Source
                {
                    Id = 1,
                    Name = "Source1",
                    Url = Guid.NewGuid().ToString(),
                }, new Source
                {
                    Id = 2,
                    Name = "Source2",
                    Url = Guid.NewGuid().ToString(),
                }, new Source
                {
                    Id = 3,
                    Name = "Source3",
                    Url = Guid.NewGuid().ToString(),
                });

            dbTestContext.Subscriptions.AddRange(
                new Subscription
                {
                    SourceId = 1,
                    UserId = 1,
                    UserGroupId = 1
                }, new Subscription
                {
                    SourceId = 3,
                    UserId = 1,
                    UserGroupId = 1
                }, new Subscription
                {
                    SourceId = 2,
                    UserId = 1,
                    UserGroupId = 2
                }, new Subscription
                {
                    SourceId = 1,
                    UserId = 2,
                    UserGroupId = 3
                });

            dbTestContext.UserGroups.AddRange(
                new UserGroup
                {
                    Id = 1,
                    Name = "Group1",
                }, new UserGroup
                {
                    Id = 2,
                    Name = "Group2",
                }, new UserGroup
                {
                    Id = 3,
                    Name = "Group3",
                });
            dbTestContext.SaveChanges();

            var mockDbFactory = new Mock<IDbContextFactory<DataContext>>();
            mockDbFactory.Setup(f => f.CreateDbContextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbTestContext);

            UserGroupLogic userGroupLogic = new UserGroupLogic(mockDbFactory.Object);

            // Act
            var actualUserSubsciptions = await userGroupLogic.GetAllByUserAsync(1);

            //Assert
            Assert.That(actualUserSubsciptions, Is.Not.Null.Or.Empty);
            Assert.That(actualUserSubsciptions.Count, Is.EqualTo(2));
            Assert.That(actualUserSubsciptions.First().Subscriptions.Count, Is.EqualTo(2));
            Assert.That(actualUserSubsciptions.Last().Subscriptions.Count, Is.EqualTo(1));
        }
    }
}
