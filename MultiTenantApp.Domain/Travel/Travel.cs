using MultiTenantApp.Domain.Common;
using System;

namespace MultiTenantApp.Domain.Travel
{
    public class Travel : Entity<int>
    {
        public Travel()
        {
        }
        public Travel(string name, string description, User user)
        {
            Name = name;
            Description = description;
            CreatedBy = user;
            CreatedAt=DateTime.Now;
        }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public User CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public User ModifiedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public void ChangeDescription(string updatedDescription, User modifiedBy)
        {
            Description = updatedDescription;
            ModifiedBy = modifiedBy;
            UpdatedAt = DateTime.Now;
        }
    }
}