using CipatBarzel.Utils;
using CipatBarzel.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CipatBarzel.Models
{
    public class Threat

    {
        public Threat() 
        {
            Status = ThreatStatus.inActive;
        }
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public int ResponceTime 
        {
            get
            {
                return (TerrorOrg.Distance / Type.Speed) * 3600;
            }
        }

        [Display(Name = "מקור האיום")]
        public TerrorOrg TerrorOrg { get; set; }

        [Display(Name = "סוג האיום")]
        public ThreatAmmunition Type { get; set; }

        [Display(Name = "פעיל")]
		public ThreatStatus Status { get; set; } // inActive // active // failed // succeeded
        public DateTime FireTime { get; set; }

		


    }
}
