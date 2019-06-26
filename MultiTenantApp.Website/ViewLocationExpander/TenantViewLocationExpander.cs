using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MultiTenantApp.Website.ViewLocationExpander
{
    public class TenantViewLocationExpander: IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            throw new System.NotImplementedException();
        }
    }
}