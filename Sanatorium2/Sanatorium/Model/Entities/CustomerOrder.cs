using System.ComponentModel.DataAnnotations;

namespace Sanatorium.Model.Entities
{
    public class CustomerOrder
    {
        public int Id { get; set; }

        [Required]
        public Customer? Customer { get; set; }

        public TreatmentProgram? TreatmentProgram { get; set; }

        [Required]
        public Room? Room { get; set; }

    }
}
