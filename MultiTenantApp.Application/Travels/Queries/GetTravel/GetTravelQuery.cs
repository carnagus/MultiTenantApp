using System;

namespace MultiTenantApp.Application.Travels.Queries.GetTravel
{
    using MediatR;
    using MultiTenantApp.Application.Travels.Queries.ViewModels;
    public class GetTravelQuery: IRequest<TravelViewModel>
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}