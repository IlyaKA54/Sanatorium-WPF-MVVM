using System.ComponentModel.DataAnnotations;

namespace Sanatorium.Model.Entities
{
    public class TypeOfRoom
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
