using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareNetCoreSample.Models
{
    public enum ClaimStatus
    {
        open, closed, waiting
    }

    public class Claim
    {
        [Display(Name = "Claim ID")]
        public int ClaimID { get; set; }
        [Display(Name = "Patient ID")]
        public int PatientID { get; set; }
        public int InsProviderID { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "Amount Owed")]
        public decimal AmountOwed { get; set; }
        [DisplayFormat(NullDisplayText = "Not set")]
        public ClaimStatus? ClaimStatus { get; set; }
        [Display(Name = "Insurance Provider")]
        public InsProvider InsProvider { get; set; }
        [Display(Name = "Patient Name")]
        public Patient Patient { get; set; }


    }
}
