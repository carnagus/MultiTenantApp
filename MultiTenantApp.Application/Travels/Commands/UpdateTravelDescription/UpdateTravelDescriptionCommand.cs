using System;
using MediatR;

namespace MultiTenantApp.Application.Travels.Commands.UpdateTravelDescription
{
    public class UpdateTravelDescriptionCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}