using System;

namespace MultiTenantApp.Application.Travels.Queries.GetAllTravels
{
    using MediatR;
    using MultiTenantApp.Application.Travels.Queries.ViewModels;
    public class GetAllTravelsQuery : IRequest<TravelsListViewModel>
    {
        public Guid UserId { get; set; }
        public bool IsAdmin { get; set; }
    }
}