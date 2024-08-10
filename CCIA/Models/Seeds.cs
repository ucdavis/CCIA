using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{

     public enum SeedsStatus
    { 
        [Display(Name="Pending supporting material")]
        PendingSupportingMaterial,
        [Display(Name="Pending Lab Results")]
        PendingLabResults,
        [Display(Name="Pending documentation upload")]
        PendingDocumentation,
        [Display(Name="Pending Final Submission")]
        PendingFinalSubmission,
        [Display(Name="Pending acceptance")]
        PendingAcceptance,
        [Display(Name="SIR ready")]
        SIRReady,        
        [Display(Name="Cancelled by organization")]
        CancelledByOrganization,        
        [Display(Name="Cancelled by CCIA")]
        CancelledByCCIA,
        [Display(Name ="Returned to Client")]
        ReturnedToClient
    } 
    public partial class Seeds
    {

        [Display(Name = "SID")] 
        public int Id { get; set; }
        [ForeignKey("CertProgram")]
        public AbbrevAppType AppTypeTrans { get; set; }
        [Display(Name = "Cert Program")] 
        public string CertProgram { get; set; }
        public int? AppId { get; set; }
        [ForeignKey("AppId")]
        public Applications Application { get; set; }
        public int? SampleFormNumber { get; set; }
        [Display(Name = "Sample Form Date")] 
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SampleFormDate { get; set; }
        public string SampleFormCertNumber { get; set; }
        public int? SampleFormRad { get; set; }
        [Display(Name = "Cert Year")] 
        public int? CertYear { get; set; }

        [ForeignKey("ApplicantId")]
        public Organizations ApplicantOrganization { get; set; }
        public int? ApplicantId { get; set; }

        [ForeignKey("ConditionerId")]
        public Organizations ConditionerOrganization { get; set; }
        public int ConditionerId { get; set; }

        public int? SampleFormVarietyId { get; set; }

        [ForeignKey("OfficialVarietyId")]
        public VarFull Variety { get; set; }
        public int? OfficialVarietyId { get; set; }
        //public VarFull  Variety { get; set; }

        [Display(Name="Lot Number")]        
        public string LotNumber { get; set; }
        [Display(Name="Lot Weight (pounds)")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:N1}")]
        public decimal PoundsLot { get; set; }

        [ForeignKey("Class")]
        public AbbrevClassSeeds ClassProduced { get; set; }
        public int? Class { get; set; }
        public int? ClassAccession { get; set; }
        public string Status { get; set; }
        [Display(Name="County Drawn")]
        public int? CountyDrawn { get; set; }
        [ForeignKey("CountyDrawn")]
        public County CountySampleDrawn { get; set; }
        public int? OriginState { get; set; }
        [ForeignKey("OriginState")]
        public StateProvince StateOfOrigin { get; set; }
        public int? OriginCountry { get; set; }
        [ForeignKey("OriginCountry")]
        public Countries CountryOfOrigin { get; set; }
        public bool Bulk { get; set; }
        public bool OriginalRun { get; set; }
        public bool Remill { get; set; }
        public bool Treated { get; set; }
        public bool OECDTestRequired { get; set; }
        public bool Resampled { get; set; }
        
        public string Remarks { get; set; }
        public string SampleDrawnBy { get; set; }
        public string SamplerID { get; set; }
     
        public int? SampleId { get; set; }
        [Display(Name="OECD Lot?")]
        public bool OECDLot { get; set; }
        public bool Rush { get; set; }
        public bool InDirt { get; set; }
        public int? BlendNumber { get; set; }

        [Display(Name = "Date Rec'd.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateSampleReceived { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? CropFee { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? CertFee { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? ResearchFee { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? MinimumFee { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C}")]
        public decimal? TotalFee
        {
            get
            {
                return CropFee.GetValueOrDefault() + CertFee.GetValueOrDefault() + ResearchFee.GetValueOrDefault() + MinimumFee.GetValueOrDefault();
            }

        }

        [Display(Name = "Finally Certified by Other Agency?")]
        public bool CertByOtherAgency { get; set; }
        public bool LotCertOk { get; set; }
        [Display(Name="Entered By")]
        public int? UserEntered { get; set; }
        [ForeignKey("UserEntered")]
        public Contacts ContactEntered { get; set; }
        public bool Submitted { get; set; }
        public bool Confirmed { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        //public int? YearConfirmed => ConfirmedAt != null ? ConfirmedAt.Value.Year : 0;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int YearConfirmed { get; set; }
        public bool Docs { get; set; }
        public string EmployeeModified { get; set; }
        [Display(Name = "Not Finally Certified")] 
        public bool NotFinallyCertified { get; set; }
         [Display(Name = "Charge Full Fees")]
        public bool ChargeFullFees { get; set; }

        public List<SeedsApplications>   SeedsApplications { get; set; }

        public List<SeedDocuments> Documents { get; set; }

        [ForeignKey("Id")]
        public SampleLabResults LabResults { get; set; }

        [ForeignKey("Id")]
        public List<OECD> OECDForm { get; set; }

        [ForeignKey("SID")]
        public ICollection<SeedsChanges> Changes { get; set; }

        public bool FollowUp { get; set; }
        public bool Sublot { get; set; }
        [Display(Name ="Return Reason")]
        public string ReturnReason { get; set; }
       

        public bool HasLabs => LabResults == null || (LabResults.PurityPercent == null && LabResults.GermPercent == null) ? false : true;

        public string certYearAbbrev => CertYear != null ? CertYear.ToString().Substring(CertYear.ToString().Length - 2) : "";

        // NO lot number included
        [Display(Name = "Cert#")]
        public string CertNumber
        {
            get
            {                
                if (OriginState != 102 || AppId != null)
                {
                    if (AppId != null)
                    {
                        return SampleFormCertNumber;
                    }
                    if (OriginState != 102)
                    {
                        return SampleFormCertNumber;
                    }
                    if(Variety == null)
                    {
                        return "unknown";
                    }
                    if (CertYear < 2007)
                    {
                        return $"{certYearAbbrev}{Variety.Crop.Annual}-{SampleFormCertNumber}";
                    }
                    return $"{certYearAbbrev}CA-{SampleFormCertNumber}";
                }
                else
                {
                    if (CertYear < 2007 || SampleFormRad == null)
                    {
                        if(Variety == null || Variety.Crop == null)
                        {
                            return "unknown";
                        }
                        return $"{certYearAbbrev}{Variety.Crop.Annual}-{SampleFormCertNumber}";
                    }                   
                    return $"{certYearAbbrev}CA-{SampleFormRad}-{SampleFormCertNumber}";
                }

            }
        }

        public string FullCert() {
            return $"{CertNumber}-{LotNumber}";
        }

        public string GetCropName()
        {            
            if (AppId.HasValue)
            {
                if(Application.SubspeciesName != "")
                {
                    return $"{Application.CropName} (Subspecies: {Application.SubspeciesName}";
                }
                return Application.CropName;
            }
            if(Variety != null && Variety.Crop != null)
            {
                if(Variety.Subspecies != null)
                {
                    return $"{Variety.Crop.Name} (Subspecies: {Variety.Subspecies.Name})";
                }
                return Variety.Crop.Name;
            }
            return "";
        }

        public int GetCropId()
        { 
            if(AppId.HasValue)
            {
                return Application.CropId.Value;
            }
            return Variety == null ? 0 : Variety.CropId;
            
        }

        public string GetVarietyName()
        {
            if (AppId.HasValue)
            {
                return Application.VarietyName;
            }
            return Variety == null ? "" : Variety.Name;
        }

        public string CertResults()
        {
            if(NotFinallyCertified && CertProgram == "NS" && Class == 80)
            {
                return "Passed for G1 increase";
            }
            if(NotFinallyCertified)
            {
                return "REJECTED - LOT 'Not Finally Certified'";
            }
            if(LabResults == null)
            {
                return "REJECTED";
            }
            if(LabResults.PurityResults == "P" && LabResults.GermResults == "P" && (LabResults.AssayResults == "P" || LabResults.AssayResults == "N"))
            {
                return "PASSED";
            }
            if(LabResults.PurityResults == "S" || LabResults.GermResults == "S" || LabResults.AssayResults == "S")
            {
                return "Passed - Substandard";
            }
            return "REJECTED";
        }

        public string VarietyComment()
        {
            if(CertProgram == "GQ" || CertProgram == "PV" || CertProgram == "RQ" || CertProgram == "LT")
            {
                return "";
            }
            if(Variety != null && Variety.Certified)
            {
                return "";
            }
            return "Variety pending CCIA Board Approval - no tags can be issued until variety approval is complete";
        }


        public string CertificateCornerLabel()
        {            
            int end = SampleId.HasValue ? SampleId.Value : 0;

            return $"{certYearAbbrev}-{FormNumber()}-{end}";


        }

        public string FormNumber()
        {
            string IdString = "";
            
            if(SampleFormNumber.HasValue)
            {
                IdString =SampleFormNumber.Value.ToString();
            } else 
            {
                IdString = $"SID {Id}";
            }
            return IdString;

        }

        public string CropFeeLabel()
        {
            if(!CropFee.HasValue || CropFee.Value == 0)
            {
                return "";
            }
            if(Variety != null && Variety.CropId == 1)
            {
                return "National Alfalfa Alliance Fee";
            }
            if(Variety != null && Variety.Crop != null)
            {
                return $"{Variety.Crop.Name} Fee";
            }
            return "";

        }

        






    }
}
