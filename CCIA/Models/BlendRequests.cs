﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;

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
        [Display(Name="Cancelled")]
        Cancelled,
		[Display(Name = "Returned to Client")]
		ReturnedToClient,
	} 

    public enum BlendType
    {
        [Display(Name="Lot")]
        Lot,
        [Display(Name="Varietal")]
        Varietal,
        [Display(Name="In Dirt")]
        InDirt,
    }

    public partial class BlendRequests
    {
        public BlendRequests()
        {
            Sublot = false;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
        [Display(Name ="Blend Type")]
        public string BlendType { get; set; }
        [Display(Name ="Requested")]
        public DateTime RequestStarted { get; set; }
        public bool Sublot { get; set; }
        public int? ParentId { get; set; }
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

        public string SublotNumber { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool? Submitted { get; set; }
        [Display(Name = "Return Reason")] 
        public string ReturnReason { get; set; }
        public bool? Approved { get; set; }
        [Display(Name ="Approved")]
        public DateTime? ApproveDate { get; set; }
        public string ApprovedBy { get; set; }

        [ForeignKey("BlendId")]
        public ICollection<LotBlends> LotBlends { get; set; }

        [ForeignKey("Id")]
        public BlendLabResults Labs { get; set; }

        [ForeignKey("ApprovedBy")]
        public CCIAEmployees ApprovedByEmployee { get; set; }

        [ForeignKey("BlendId")]
        public ICollection<BlendInDirtComponents> InDirtBlends { get; set; }

        [ForeignKey("VarietyId")]
        public VarFull Variety { get; set; }

        [ForeignKey("ConditionerId")]
        public Organizations Conditioner { get; set; }

        [ForeignKey("UserEntered")]
        public Contacts EnteredByContact { get; set; }

        [ForeignKey("ParentId")]
        public BlendRequests ParentBlend { get; set; }

        public bool FollowUp { get; set; }

        [Display(Name ="Crop")]
        public string GetCrop()
        {
            if ((BlendType == "Varietal" && Variety != null) || (BlendType == "Lot" && Sublot))
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
            if ((BlendType == "Varietal" && Variety != null) || (BlendType == "Lot" && Sublot))
            {
                return Variety.Name;
            }
            if (BlendType == "Lot" && LotBlends.Any() && LotBlends.First().Seeds != null && LotBlends.First().Seeds.Variety != null)
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

        [Display(Name ="Variety Id")]
        public int GetVarietyId()
        {           
            if ((BlendType == "Varietal" && Variety != null) || (BlendType == "Lot" && Sublot))
            {
                return Variety.Id;
            }
            if (BlendType == "Lot" && LotBlends.Any() && LotBlends.First().Seeds != null && LotBlends.First().Seeds.Variety != null)
            {
                return LotBlends.First().Seeds.Variety.Id;
            }          
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().AppId != null && InDirtBlends.First().Application != null && InDirtBlends.First().Application.Variety != null)
            {
                return InDirtBlends.First().Application.Variety.Id;
            }
            if (BlendType == "In Dirt" && InDirtBlends.Any() && InDirtBlends.First().Variety != null)
            {
                return InDirtBlends.First().Variety.Id;
            }
            return -1;
        }

        [Display(Name ="Cert Year")]
        public int CertYear
        {
            get
            {
                var date = new DateTime();
                if(ParentBlend != null)
                {
                    date = ParentBlend.RequestStarted;
                } else
                {
                    date = RequestStarted;
                }
                if (date.Date.Month == 10 || date.Date.Month == 11 || date.Date.Month == 12)
                {
                    return date.Date.Year + 1;
                }
                else
                {
                    return date.Date.Year;
                }
            }
        }

        [Display(Name ="Cert #")]
        public string CertNumber
        {
            get
            {
                var twoDigitYear = CertYear.ToString().Substring(CertYear.ToString().Length - 2, 2);
                if (Sublot)
                {
                    return $"CA-L{twoDigitYear}{ParentBlend.Id}-{SublotNumber}";
                }
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

        public string BlendDefinition
        {
            get
            {
                switch(BlendType)
                {
                    case "Lot":
                        return "Different lots of the same variety";
                    case "Varietal":
                        return "Mix of different varieties in previously approved ratios";
                    case "In Dirt":
                        return "Different lots/cert numbers direct from field harvest";
                }
                return "";
            }
        }

        



    }
}
