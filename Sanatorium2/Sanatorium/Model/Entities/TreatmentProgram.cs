using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanatorium.Model.Entities
{
    public class TreatmentProgram
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
    }
}
