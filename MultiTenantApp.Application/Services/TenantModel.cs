namespace MultiTenantApp.Application.Services
{
    public class TenantModel
    {
        public string DomainName { get; set; }
        public string ConnectionString { get; set; }
        public bool Default { get; set; }
    }
}