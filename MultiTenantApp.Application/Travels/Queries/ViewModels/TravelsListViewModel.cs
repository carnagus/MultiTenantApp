using System.Collections.Generic;

namespace MultiTenantApp.Application.Travels.Queries.ViewModels
{
    public class TravelsListViewModel
    {
        public IEnumerable<TravelViewModel> Travels { get; set;}
    }
}