using Microsoft.EntityFrameworkCore;
using Moq;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Models;
using NUnit.Framework;

namespace News.Vampire.Service.Tests.BusinessLogic
{
    public class GroupLogicTests: BaseTests
    {
        [TearDown]
        public void CleanUp()
        {
            var dbTestContext = new DataContext(GetDbContextOptions(), unitTestMode: true);
            dbTestContext.Groups.RemoveRange(dbTestContext.Groups);
        }

        [Test]
        public async Task GetAllTest()
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
