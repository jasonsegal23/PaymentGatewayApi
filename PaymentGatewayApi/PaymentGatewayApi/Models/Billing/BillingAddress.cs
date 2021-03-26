using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Models.Billing
{
    public class BillingAddress
    {
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string Address1 { get; set; }    

        public string Address2 { get; set; }

        public string Postcode { get; set; }
    }
}
