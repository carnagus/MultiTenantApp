using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MultiTenantApp.Application.Interfaces;
using MultiTenantApp.Application.Mappers;
using MultiTenantApp.Application.Travels.Queries.ViewModels;

namespace MultiTenantApp.Application.Travels.Queries.GetTravel
{
    public class GetTravelQueryHandler: IRequestHandler<GetTravelQuery, TravelViewModel>
    {
        private readonly ITravelDbContext _travelDbContext;
        public GetTravelQueryHandler(ITravelDbContext travelDbContext)
        {
            _travelDbContext = travelDbContext;
        }
        public async Task<TravelViewModel> Handle(GetTravelQuery request, CancellationToken cancellationToken)
        {
            var travel = await _travelDbContext.Travels.FindAsync(request.Id);
            var travelViewModel = travel?.ToTravelViewModel(request.IsAdmin, request.UserId);

            return travelViewModel;
        }
    }
}