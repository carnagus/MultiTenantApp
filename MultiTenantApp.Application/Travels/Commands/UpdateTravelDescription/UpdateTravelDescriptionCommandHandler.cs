using MediatR;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Domain.Travel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantApp.Application.Travels.Commands.UpdateTravelDescription
{
    public class UpdateTravelDescriptionCommandHandler: IRequestHandler<UpdateTravelDescriptionCommand, int>
    {
        private const string NO_TRAVEL = "Travel with this id does not exists";
        private readonly ITravelDbContext _travelDbContext;
        public UpdateTravelDescriptionCommandHandler(ITravelDbContext travelDbContext)
        {
            _travelDbContext = travelDbContext;
        }
        public async Task<int> Handle(UpdateTravelDescriptionCommand request, CancellationToken cancellationToken)
        {
            var travel = await _travelDbContext.Travels.FindAsync(request.Id);
            if(travel==null)
                throw new ApplicationException(NO_TRAVEL);
            
             var user = await _travelDbContext.Users.FindAsync(request.UserId) ?? new User(request.UserId,request.Email);

            if (request.IsAdmin|| travel.CreatedBy.Id==request.UserId)
                    travel.ChangeDescription(request.Description,user);

            await _travelDbContext.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}