using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanatorium.Model.Entities
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public TypeOfRoom? Type { get; set; }

        public int NumberOfPlaces { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        public byte[] Image { get; set; }

        public string Description { get; set; }
    }
}
