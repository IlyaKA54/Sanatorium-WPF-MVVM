using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Sanatorium.Model.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public byte[] Image { get; set; } 

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public int NumberOfVisits { get; set; }
    }
}
