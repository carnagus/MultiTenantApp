namespace MultiTenantApp.Application.Travels.Commands.CreateTravel
{
    using MediatR;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using MultiTenantApp.Application.Interfaces;
    using MultiTenantApp.Domain.Travel;
    using System.Threading;
    using System.Threading.Tasks;
    public class CreateTravelCommandHandler: IRequestHandler<CreateTravelCommand, int>
    {
        private const string TEST_EMAIL = "marcin.mazurowski@o2.pl";
        private readonly ITravelDbContext _travelDbContext;
        private readonly ISendGridService _sendGridService;
        public CreateTravelCommandHandler(ITravelDbContext travelDbContext, ISendGridService sendGridService)
        {
            _travelDbContext = travelDbContext;
            _sendGridService = sendGridService;
        }
        public async Task<int> Handle(CreateTravelCommand request, CancellationToken cancellationToken)
        {
            var user = _travelDbContext.Users.Find(request.UserId);
            Travel travel;
            EntityEntry<Travel> result;
            if (user == null)
            {
                travel = new Travel(request.Name, request.Description, new User(request.UserId, request.Email));
                result = await _travelDbContext.Travels.AddAsync(travel,cancellationToken);
                await _travelDbContext.SaveChangesAsync(cancellationToken);
                await _sendGridService.SendToSingleRecipientAsync(TEST_EMAIL, TEST_EMAIL,
                    request.Name, string.Empty, request.Description);

                return result.Entity.Id;
            }
            user.ChangeEmail(request.Email);
            travel = new Travel(request.Name, request.Description, user);
            result = await _travelDbContext.Travels.AddAsync(travel, cancellationToken);
            await _travelDbContext.SaveChangesAsync(cancellationToken);
            await _sendGridService.SendToSingleRecipientAsync(TEST_EMAIL, TEST_EMAIL,
                request.Name, string.Empty, request.Description);

            return result.Entity.Id;
        }
    }
}