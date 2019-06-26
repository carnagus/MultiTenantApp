namespace MultiTenantApp.Application.Travels.Queries.GetAllTravels
{
    using MediatR;
    using MultiTenantApp.Application.Interfaces;
    using MultiTenantApp.Application.Travels.Queries.ViewModels;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MultiTenantApp.Application.Mappers;
    using System.Linq;
    public class GetAllTravelsQueryHandler: IRequestHandler<GetAllTravelsQuery, TravelsListViewModel>
    {
        private readonly ITravelDbContext _travelDbContext;
        public GetAllTravelsQueryHandler(ITravelDbContext travelDbContext)
        {
            _travelDbContext = travelDbContext;
        }
        public async Task<TravelsListViewModel> Handle(GetAllTravelsQuery request, CancellationToken cancellationToken)
        {
            var travels = await _travelDbContext.Travels.Select(x=> x.ToTravelViewModel(request.IsAdmin,request.UserId)).ToListAsync(cancellationToken);
            var result = new TravelsListViewModel
            {
                Travels = travels
            };

            return result;
        }
    }
}