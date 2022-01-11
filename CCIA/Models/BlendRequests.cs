using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CCIA.Models
{
     public enum BlendStatus
    { 
        [Display(Name="Initiated")]
        Initiated,
        [Display(Name="Pending acceptance")]
        PendingAcceptance,
        [Display(Name="Approved")]
        Approved,        
    } 

    public partial class BlendRequests
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Blend Type")]
        public string BlendType { get; set; }
        [Display(Name ="Requested")]
        public DateTime RequestStarted { get; set; }
        public int ConditionerId { get; set; }
        public int UserEntered { get; set; }
        public decimal? LbsLot { get; set; }
        public int? Class { get; set; }
        public string Status { get; set; }
        public int? TagCountRequested { get; set; }
        public int? TagType { get; set; }
        public int? VarietyId { get; set; }
        public DateTime? DateNeeded { get; set; }
        public string HowDeliver { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comments { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool? Submitted { get; set; }
        public bool? Approved { get; set; }
        [Display(Name ="Approved")]
        public DateTime? ApproveDate { get; set; }
        public string ApprovedBy { get; set; }

        [ForeignKey("BlendId")]
        public ICollection<LotBlends> LotBlends { get; set; }

        [ForeignKey("BlendId")]
        public ICollection<BlendInDirtComponents> InDirtBlends { get; set; }

        [ForeignKey("VarietyId")]
        public VarFull Variety { get; set; }

        [ForeignKey("ConditionerId")]
        public Organizations Conditioner { get; set; }

        [Display(Name ="Crop")]
        public string GetCrop()
        {
            if (BlendType == "Varietal" && Variety != null)
            {
                return Variety.Crop.Name;
            }
            if (BlendType == "Lot" && LotBlends.Any() && LotBlends.First().Seeds.Variety.Crop != null)
            {
                return LotBlends.First().Seeds.Variety.Crop.Name;
            }
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().Application.Crop != null)
            {
                return InDirtBlends.First().Application.Crop.Name;
            }
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().Crop != null)
            {
                return InDirtBlends.First().Crop.Name;
            }
            return "unknown";
        }

        [Display(Name ="Variety")]
        public string GetVarietyName()
        {           
            if (BlendType == "Varietal" && Variety != null)
            {
                return Variety.Name;
            }
            if (BlendType == "Lot" && LotBlends.Any() && LotBlends.First().Seeds.Variety != null)
            {
                return LotBlends.First().Seeds.Variety.Name;
            }          
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().AppId != null && InDirtBlends.First().Application != null && InDirtBlends.First().Application.Variety != null)
            {
                return InDirtBlends.First().Application.Variety.Name;
            }
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().Variety != null)
            {
                return InDirtBlends.First().Variety.Name;
            }
            return "unknown";
        }

        [Display(Name ="Cert Year")]
        public int CertYear
        {
            get
            {
                if (RequestStarted.Date.Month == 10 || RequestStarted.Date.Month == 11 || RequestStarted.Date.Month == 12)
                {
                    return RequestStarted.Date.Year + 1;
                }
                else
                {
                    return RequestStarted.Date.Year;
                }
            }
        }

        [Display(Name ="Cert #")]
        public string CertNumber
        {
            get
            {
                var twoDigitYear = CertYear.ToString().Substring(CertYear.ToString().Length - 2, 2);
                switch (BlendType)
                {
                    case "Lot":
                        return $"CA-L{twoDigitYear}{Id}";
                    case "Varietal":
                        return $"CA-V{twoDigitYear}{Id}";
                    case "In Dirt":
                        return $"CA-D{twoDigitYear}{Id}";
                }
                return "";
            }
        }



    }
}
