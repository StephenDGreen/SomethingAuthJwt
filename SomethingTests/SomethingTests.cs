using Moq;
using Something.Application;
using Something.Domain;
using Something.Persistence;
using Something.Security;
using SomethingTests.Infrastructure.Factories;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain = Something.Domain.Models;

namespace SomethingTests
{
    public class SomethingTests
    {
        private readonly Domain.Something something = new Domain.Something() { Name = "Fred Bloggs" };

        [Fact]
        public void Something_HasAName()
        {
            var something1 = new Domain.Something();
            string expected = null;

            string actual = something1.Name;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SomethingFactory_Create_CreatesSomething()
        {
            SomethingFactory somethingFactory = new SomethingFactory();

            Domain.Something actual = somethingFactory.Create();

            Assert.IsType<Domain.Something>(actual);
        }
        [Fact]
        public void SomethingFactory_Create_CreatesSomethingWithName()
        {
            SomethingFactory somethingFactory = new SomethingFactory();
            string expected = "Fred Bloggs";

            Domain.Something actual = somethingFactory.Create(expected);

            Assert.IsType<Domain.Something>(actual);
            Assert.Equal(expected, actual.Name);
        }
        [Fact]
        public void DbContextFactory_CreateAppDbContext_SavesSomethingToDatabaseAndRetrievesIt()
        {

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingToDatabaseAndRetrievesIt)))
            {
                ctx.Somethings.Add(something);
                ctx.SaveChanges();
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(DbContextFactory_CreateAppDbContext_SavesSomethingToDatabaseAndRetrievesIt)))
            {
                var savedSomething = ctx.Somethings.Single();
                Assert.Equal(something.Name, savedSomething.Name);
            };
        }

        [Fact]
        public void SomethingPersistence__SaveSomething__SavesSomethingToDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingPersistence__SaveSomething__SavesSomethingToDatabase)))
            {
                var persistence = new SomethingPersistence(ctx);
                persistence.SaveSomething(something);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingPersistence__SaveSomething__SavesSomethingToDatabase)))
            {
                var savedSomething = ctx.Somethings.Single();
                Assert.Equal(something.Name, savedSomething.Name);
            };
        }

        [Fact]
        public void SomethingPersistence__GetSomethingList__RetrievesSomethingListFromDatabase()
        {
            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingPersistence__GetSomethingList__RetrievesSomethingListFromDatabase)))
            {
                var persistence = new SomethingPersistence(ctx);
                persistence.SaveSomething(something);
            };

            using (var ctx = new DbContextFactory().CreateAppDbContext(nameof(SomethingPersistence__GetSomethingList__RetrievesSomethingListFromDatabase)))
            {
                var persistence = new SomethingPersistence(ctx);
                var savedSomething = persistence.GetSomethingList();
                Assert.Equal(something.Name, savedSomething.Single().Name);
            };
        }

        [Fact]
        public void SomethingCreateInteractor_CreateSomething_PersistsSomething()
        {
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create()).Returns(something);
            Mock<ISomethingPersistence> mockPersistence = new Mock<ISomethingPersistence>();
            SomethingCreateInteractor somethingInteractor = new SomethingCreateInteractor(mockSomethingFactory.Object, mockPersistence.Object);

            somethingInteractor.CreateSomething();

            mockPersistence.Verify(x => x.SaveSomething(something));
        }

        [Fact]
        public void SomethingCreateInteractor_CreateSomething_PersistsSomethingWithName()
        {
            Mock<ISomethingFactory> mockSomethingFactory = new Mock<ISomethingFactory>();
            mockSomethingFactory.Setup(x => x.Create(something.Name)).Returns(something);
            Mock<ISomethingPersistence> mockPersistence = new Mock<ISomethingPersistence>();
            SomethingCreateInteractor somethingInteractor = new SomethingCreateInteractor(mockSomethingFactory.Object, mockPersistence.Object);

            somethingInteractor.CreateSomething(something.Name);

            mockPersistence.Verify(x => x.SaveSomething(something));
        }

        [Fact]
        public void SomethingReadInteractor_ReadSomethingList_RetrievesSomethingListFromPersistence()
        {
            var somethingList = new List<Domain.Something>();
            somethingList.Add(something);
            var mockPersistence = new Mock<ISomethingPersistence>();
            mockPersistence.Setup(x => x.GetSomethingList()).Returns(somethingList);
            SomethingReadInteractor interactor = new SomethingReadInteractor(mockPersistence.Object);
            
            List<Domain.Something> somethingList1 = interactor.GetSomethingList();
            
            Assert.Equal(somethingList.Count, somethingList1.Count);
            Assert.Equal(somethingList[somethingList.Count - 1].Name, somethingList1[somethingList1.Count - 1].Name);
        }

        [Fact]
        public void UserManager_GetUserPrinciple_ReturnsUserPrinciple()
        {
            var userManager = new SomethingUserManager();

            var actual = userManager.GetUserPrinciple();

            Assert.IsType<System.Security.Claims.ClaimsPrincipal>(actual);
        }
    }
}
