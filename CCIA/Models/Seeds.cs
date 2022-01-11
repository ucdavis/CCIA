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
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
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
        public DateTime? DateSampleReceived { get; set; }
        public decimal? CropFee { get; set; }
        public decimal? CertFee { get; set; }
        public decimal? ResearchFee { get; set; }
        public decimal? MinimumFee { get; set; }
        
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
        public bool ChargeFullFees { get; set; }

        public List<SeedsApplications>   SeedsApplications { get; set; }

        public List<SeedDocuments> Documents { get; set; }

        [ForeignKey("Id")]
        public SampleLabResults LabResults { get; set; }

        [ForeignKey("Id")]
        public List<OECD> OECDForm { get; set; }

        [ForeignKey("SID")]
        public ICollection<SeedsChanges> Changes { get; set; }
       

        public bool HasLabs => LabResults == null || (LabResults.PurityPercent == null && LabResults.GermPercent == null) ? false : true;

        // NO lot number included
        [Display(Name = "Cert#")]
        public string CertNumber
        {
            get
            {
                string certYearAbbrev = CertYear.ToString().Substring(CertYear.ToString().Length - 2);
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
                return Application.CropName;
            }
            return Variety == null || Variety.Crop == null ? "" : Variety.Crop.Name;
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




    }
}
