using MultiTenantApp.Domain.Common;

namespace MultiTenantApp.Domain.Catalog
{
    public class TenantConfiguration:Entity<int>
    {
        public string Name { get; private set; }
        public string DomainName { get; private set; }
        public string ConnectionString { get; private set; }
        public bool Default { get; private set; }
    }
}