using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class MyCustomers
    {
         public MyCustomers() {
           CountryId = 58;
           StateId = 102;
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public Organizations Organization { get; set; }
        public string Name { get; set; }

        [Display(Name="Address 1")]
        public string Address1 { get; set; }

        [Display(Name="Address 2")]
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public StateProvince State { get; set; }
        public int? CountyId { get; set; }
        [ForeignKey("CountyId")]
        public County County { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
