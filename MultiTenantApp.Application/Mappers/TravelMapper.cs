using MultiTenantApp.Application.Travels.Queries.ViewModels;
using MultiTenantApp.Domain.Travel;
using System;

namespace MultiTenantApp.Application.Mappers
{
    public static class TravelMapper
    {
        public static TravelViewModel ToTravelViewModel(this Travel travel, bool isAdmin, Guid editingUserId)
        {
            return new TravelViewModel
            {
                Id = travel.Id,
                Name = travel.Name,
                Description = travel.Description,
                IsEditable = isAdmin || travel.CreatedBy.Id == editingUserId
            };
        }
    }
}