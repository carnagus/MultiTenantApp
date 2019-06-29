namespace MultiTenantApp.Application.Tests.Travels.Queries
{
    using MultiTenantApp.Application.Tests.Infrastructure;
    using MultiTenantApp.Application.Travels.Queries.GetAllTravels;
    using MultiTenantApp.Application.Travels.Queries.ViewModels;
    using MultiTenantApp.Domain.Travel;
    using MultiTenantApp.Persistance.Contexts;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class GetAllTravelsQueryHandlerTest
    {
        private TravelDbContext _context;
        private GetAllTravelsQueryHandler _handler;
        private Guid _userGuid1;
        private Guid _userGuid2;
        private Guid _userGuid3;
        private Guid _userGuid4;

        [SetUp]
        public void SetUp()
        {
            _userGuid1 = Guid.NewGuid();
            _userGuid2 = Guid.NewGuid();
            _userGuid3 = Guid.NewGuid();
            _userGuid4 = Guid.NewGuid();
            _context = TravelContextFactory.Create();
            _context.Travels.AddRange(ArrangeData());
            _context.SaveChanges();
            _handler = new GetAllTravelsQueryHandler(_context);
        }

        [Test]
        public async Task HandleTestAsAdmin()
        {
            var query = new GetAllTravelsQuery { IsAdmin = true, UserId = _userGuid1 };
            var result =await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.Travels[0].IsEditable,Is.EqualTo(true));
            Assert.That(result.Travels[0].Name,Is.EqualTo("Warszawa"));
            Assert.That(result.Travels[0].Description,Is.EqualTo("Po Warszawie"));
            Assert.That(result.Travels[2].IsEditable, Is.EqualTo(true));
            Assert.That(result.Travels[2].Name, Is.EqualTo("Radom"));
            Assert.That(result.Travels[2].Description, Is.EqualTo("Po Radomiu"));
            Assert.That(result.Travels.Count, Is.EqualTo(3));
            Assert.That(result,Is.TypeOf(typeof(TravelsListViewModel)));
            Assert.That(result.Travels,Is.TypeOf(typeof(List<TravelViewModel>)));
        }

        [Test]
        public async Task HandleTestAsDiffrentUserNotAdmin()
        {
            var query = new GetAllTravelsQuery { IsAdmin = false, UserId = _userGuid4 };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.Travels[0].IsEditable, Is.EqualTo(false));
            Assert.That(result.Travels[0].Name, Is.EqualTo("Warszawa"));
            Assert.That(result.Travels[0].Description, Is.EqualTo("Po Warszawie"));
            Assert.That(result.Travels[2].IsEditable, Is.EqualTo(false));
            Assert.That(result.Travels[2].Name, Is.EqualTo("Radom"));
            Assert.That(result.Travels[2].Description, Is.EqualTo("Po Radomiu"));
            Assert.That(result.Travels.Count, Is.EqualTo(3));
            Assert.That(result, Is.TypeOf(typeof(TravelsListViewModel)));
            Assert.That(result.Travels, Is.TypeOf(typeof(List<TravelViewModel>)));
        }
        [Test]
        public async Task HandleTestAsUserFromCollectionNotAdmin()
        {
            var query = new GetAllTravelsQuery { IsAdmin = false, UserId = _userGuid1 };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.Travels[0].IsEditable, Is.EqualTo(true));
            Assert.That(result.Travels[0].Name, Is.EqualTo("Warszawa"));
            Assert.That(result.Travels[0].Description, Is.EqualTo("Po Warszawie"));
            Assert.That(result.Travels[2].IsEditable, Is.EqualTo(false));
            Assert.That(result.Travels[2].Name, Is.EqualTo("Radom"));
            Assert.That(result.Travels[2].Description, Is.EqualTo("Po Radomiu"));
            Assert.That(result.Travels.Count, Is.EqualTo(3));
            Assert.That(result, Is.TypeOf(typeof(TravelsListViewModel)));
            Assert.That(result.Travels, Is.TypeOf(typeof(List<TravelViewModel>)));
        }

        [TearDown]
        public void TearDown()
        {
            TravelContextFactory.Destroy(_context);
        }

        private Travel[] ArrangeData()
        {
            User user1 = new User(_userGuid1);
            User user2 = new User(_userGuid2);
            User user3 = new User(_userGuid3);
            var result=new Travel[]
            {
                new Travel("Warszawa", "Po Warszawie", user1),
                new Travel("Krakow", "Po Krakowie", user2),
                new Travel("Radom", "Po Radomiu", user3)
            };
            return result;
        }

    }
}