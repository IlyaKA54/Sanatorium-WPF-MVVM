using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanatorium.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        public DateTime ArrivalDate { get; set; }
        public DateTime DateOfDeparture { get; set; }
    }
}
