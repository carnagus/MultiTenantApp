namespace MultiTenantApp.Application.Tests.Travels.Queries
{
    using MultiTenantApp.Application.Tests.Infrastructure;
    using MultiTenantApp.Application.Travels.Queries.GetTravel;
    using MultiTenantApp.Application.Travels.Queries.ViewModels;
    using MultiTenantApp.Domain.Travel;
    using MultiTenantApp.Persistance.Contexts;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class GetTravelQueryHandlerTest
    {
        private TravelDbContext _context;
        private GetTravelQueryHandler _handler;
        private Guid _userGuid1;
        private Guid _userGuid2;

        [SetUp]
        public void SetUp()
        {
            _userGuid1 = Guid.NewGuid();
            _userGuid2 = Guid.NewGuid();
            _context = TravelContextFactory.Create();
            _context.Travels.Add(ArrangeData());
            _context.SaveChanges();
            _handler = new GetTravelQueryHandler(_context);
        }
        [Test]
        public async Task HandleTestAsAdmin()
        {
            var id = _context.Travels.FirstOrDefault().Id;
            var query = new GetTravelQuery { Id = id ,IsAdmin = true, UserId = _userGuid1};
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.IsEditable, Is.EqualTo(true));
            Assert.That(result.Name, Is.EqualTo("Warszawa"));
            Assert.That(result.Description, Is.EqualTo("Po Warszawie"));
            Assert.That(result, Is.TypeOf(typeof(TravelViewModel)));
        }

        [Test]
        public async Task HandleTestAsUserFromDbNotAdmin()
        {
            var id = _context.Travels.FirstOrDefault().Id;
            var query = new GetTravelQuery { Id = id, IsAdmin = false, UserId = _userGuid1 };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.IsEditable, Is.EqualTo(true));
            Assert.That(result.Name, Is.EqualTo("Warszawa"));
            Assert.That(result.Description, Is.EqualTo("Po Warszawie"));
            Assert.That(result, Is.TypeOf(typeof(TravelViewModel)));
        }
        [Test]
        public async Task HandleTestAsDiffentUserFromDbNotAdmin()
        {
            var id = _context.Travels.FirstOrDefault().Id;
            var query = new GetTravelQuery { Id = id, IsAdmin = false, UserId = _userGuid2 };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.That(result.IsEditable, Is.EqualTo(false));
            Assert.That(result.Name, Is.EqualTo("Warszawa"));
            Assert.That(result.Description, Is.EqualTo("Po Warszawie"));
            Assert.That(result, Is.TypeOf(typeof(TravelViewModel)));
        }

        [TearDown]
        public void TearDown()
        {
            TravelContextFactory.Destroy(_context);
        }

        private Travel ArrangeData()
        {
            User user1 = new User(_userGuid1);
            var result = new Travel("Warszawa", "Po Warszawie", user1);

            return result;
        }
    }
}