namespace MultiTenantApp.Application.Travels.Queries.ViewModels
{
    public class TravelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEditable { get; set; }
    }
}