using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Passion.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }

        public string DonorBlood { get; set; }
        [Column(TypeName = "Date")]

        public DateTime RegistrationDate { get; set; }


        //A keeper can take care of many animals
        public ICollection<Patient> Patients { get; set; }

    }
    public class DonorDto
    {
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }

        public string DonorBlood { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}