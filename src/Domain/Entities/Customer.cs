﻿using System.Collections.Generic;

namespace DeliveryWebApp.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string ApplicationUserFk { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
