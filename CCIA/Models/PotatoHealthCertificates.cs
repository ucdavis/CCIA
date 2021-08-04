using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
            GoldernNematode = 11;
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

       public int YearMicroPropagated { get; set; }
       public string MicropropagatedBy { get; set; }
       public int NumberOfYearsProduced { get; set; }

       public string PostHarvestLocation { get; set; }
       public int PostHarvestLeafroll { get; set; }
       public int PostHarvestMosaic { get; set; }
       public int PostHarvestOtherVarieties { get; set; }

       public int PostHarvestPlantCount { get; set; }

       public int PostHarvestSampleNumber { get; set; }

       public decimal PercentPVY { get; set; }
       public decimal PercentPVX { get; set; }

       public int BacterialRingRot { get; set; }

       public int GoldernNematode { get; set; }
       public int LateBlight { get; set; }
       public int RootKnotNematode { get; set; }
       public int PotatoRotNematode { get; set; }
       public int PotatoWart { get; set; }
       public int PowderScap { get; set; }
       public int PotatoSpindleTuberViroid { get; set; }
       public int CorkyRingSpots { get; set; }
       public string Notes { get; set; }

    }
}
