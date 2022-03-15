using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
     public enum PotatoHealthResults
    { 
        [Display(Name="0")]
        Zero,
        [Display(Name="1")]
        One,
        [Display(Name="2")]
        Two,
        [Display(Name="3")]
        Three,
        [Display(Name="4")]
        Four,
        [Display(Name="5")]
        Five,
        [Display(Name="6")]
        Six,
        [Display(Name="7")]
        Seven,
        [Display(Name="8")]
        Eight,
        [Display(Name="9")]
        Nine,
        [Display(Name="10")]
        Ten,
        [Display(Name="Not known to occur in area")]
        NotKnownToOccurInArea,
        [Display(Name="None on record")]
        NoneOnRecord,
        [Display(Name="Not found")]
        NotFound        
    } 

    public partial class PotatoHealthCertificates
    {
        public PotatoHealthCertificates() {
            LotOriginatedFromTissueCulture = false;
            PostHarvestLocation = "No post harvest test";
            BacterialRingRot = 12;
            GoldenNematode = 11;
            LateBlight = 13;
            RootKnotNematode = 13;
            PotatoRotNematode = 12;
            PotatoWart = 11;
            PowderScap = 13;
            PotatoSpindleTuberViroid = 12;
            CorkyRingSpots = 11;
            Notes = "Golden Nematode (Globodera rostochiensis) and Potato Rot Nematode (Ditylenchus destructor) are not known to be present in this area.";
        }
    
       public int AppId { get; set; }

      // public Applications Application { get; set; }

       public bool LotOriginatedFromTissueCulture { get; set; }

       [Display(Name="Micropropagate Year")]
       public int? YearMicroPropagated { get; set; }
       [Display(Name="Micropropagated By")]
       public string MicropropagatedBy { get; set; }
       [Display(Name="# Years Produced")]
       public int? NumberOfYearsProduced { get; set; }

       [Display(Name="Location")]
       public string PostHarvestLocation { get; set; }
       [Display(Name="Leafroll")]
       public int? PostHarvestLeafroll { get; set; }
       public int? PostHarvestMosaic { get; set; }
       public int? PostHarvestOtherVarieties { get; set; }

       public int? PostHarvestPlantCount { get; set; }

       public int? PostHarvestSampleNumber { get; set; }

       public decimal? PercentPVY { get; set; }
       public decimal? PercentPVX { get; set; }

       public int BacterialRingRot { get; set; }

       public int GoldenNematode { get; set; }
       public int LateBlight { get; set; }
       public int RootKnotNematode { get; set; }
       public int PotatoRotNematode { get; set; }
       public int PotatoWart { get; set; }
       public int PowderScap { get; set; }
       public int PotatoSpindleTuberViroid { get; set; }
       public int CorkyRingSpots { get; set; }
       public string Notes { get; set; }

       [ForeignKey("AppId")]
       public ICollection<PotatoHealthCertificateHistory> History { get; set; }

       
       public Applications Application { get; set; }       

    }
}
