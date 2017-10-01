using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthcareNetCoreSample.Models
{
    public class InsProvider
    {
		// Allows manual setting of primary key.
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int InsProviderID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string InsProviderName { get; set; }

        public ICollection<Claim> Claims { get; set; }
    }
}
