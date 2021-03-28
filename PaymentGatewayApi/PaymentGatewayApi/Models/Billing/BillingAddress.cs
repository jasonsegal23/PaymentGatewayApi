using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models.Billing
{
    public class BillingAddress
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Address1 { get; set; }    

        public string Address2 { get; set; }

        [Required]
        public string Postcode { get; set; }
    }
}
