using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entites.Orders
{
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }

        public Address(string firstName, string lastName, string street, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
        }

        public Address()
        {
            
        }

       
    }
}
