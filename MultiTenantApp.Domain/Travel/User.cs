using System;
using System.Collections.Generic;
using System.Text;
using MultiTenantApp.Domain.Common;

namespace MultiTenantApp.Domain.Travel
{
    public class User: Entity<Guid>
    {
        public User()
        {
        }
        public User(Guid adUserGuid)
        {
            Id = adUserGuid;
        }
        public User(Guid adUserGuid, string email)
            :this(adUserGuid)
        {
            Email = email;
        }
        public string Email { get; private set; }
        public void ChangeEmail(string email)
        {
            if (Email == email)
                return;

            Email = email;
        }
    }
}
