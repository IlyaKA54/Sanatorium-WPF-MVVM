using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject3.Data
{
    public class Customer
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }


        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }

        public int NumberOfVisits { get; set; }
    }
}
