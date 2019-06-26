using System;
using MediatR;

namespace MultiTenantApp.Application.Travels.Commands.CreateTravel
{
    public class CreateTravelCommand: IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }

    }
}