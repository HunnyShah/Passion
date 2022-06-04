using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Passion.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }

        public string PatientBlood { get; set; }

        public string DiseaseName { get; set; }
        [Column(TypeName = "Date")]

        public DateTime RegistrationDate { get; set; }


        //A keeper can take care of many animals
        public ICollection<Donor> Donors { get; set; }

    }

    public class PatientDto
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }

        public string PatientBlood { get; set; }

        public string DiseaseName { get; set; }
        public DateTime RegistrationDate { get; set; }


    }
}