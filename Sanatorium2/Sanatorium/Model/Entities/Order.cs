using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanatorium.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public List<CustomerOrder>? IdCustomerOrders { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        public DateTime ArrivalDate { get; set; }
        public DateTime DateOfDeparture { get; set; }
    }
}
