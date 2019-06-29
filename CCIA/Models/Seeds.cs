using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Seeds
    {

        [Display(Name = "SID")] 
        public int Id { get; set; }
        [ForeignKey("CertProgram")]
        public AbbrevAppType AppTypeTrans { get; set; }
        public string CertProgram { get; set; }
        public int? AppId { get; set; }
        [ForeignKey("AppId")]
        public Applications Application { get; set; }
        public int? SampleFormNumber { get; set; }
        public DateTime? SampleFormDate { get; set; }
        public string SampleFormCertNumber { get; set; }
        public int? SampleFormRad { get; set; }
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

        public string LotNumber { get; set; }
        public decimal PoundsLot { get; set; }

        [ForeignKey("Class")]
        public AbbrevClassSeeds ClassProduced { get; set; }
        public int? Class { get; set; }
        public int? ClassAccession { get; set; }
        public string Status { get; set; }
        public int? CountyDrawn { get; set; }
        public int? OriginState { get; set; }
        public int? OriginCountry { get; set; }
        public bool Bulk { get; set; }
        public bool OriginalRun { get; set; }
        public bool Remill { get; set; }
        public bool Treated { get; set; }
        public bool OECDTestRequired { get; set; }
        public bool Resampled { get; set; }
        public string CCIAAuth { get; set; }
        public string Remarks { get; set; }
        public string SampleDrawnBy { get; set; }
        public int? CertId { get; set; }
        public int? SampleId { get; set; }
        public bool OECDLot { get; set; }
        public bool Rush { get; set; }
        public bool InDirt { get; set; }
        public int? BlendNumber { get; set; }
        public DateTime? DateSampleReceived { get; set; }
        public decimal? CropFee { get; set; }
        public decimal? CertFee { get; set; }
        public decimal? ResearchFee { get; set; }
        public decimal? MinimumFee { get; set; }
        public bool BillTable { get; set; }
        public bool LotCertOk { get; set; }
        public int? UserEntered { get; set; }
        public bool Submitted { get; set; }
        public bool Confirmed { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public int? YearConfirmed => ConfirmedAt != null ? ConfirmedAt.Value.Year : 0;
        public bool Docs { get; set; }
        public string EmployeeModified { get; set; }
        public bool NotFinallyCertified { get; set; }
        public bool ChargeFullFees { get; set; }

        [ForeignKey("Id")]
        public SxLabResults LabResults { get; set; }

        public bool HasLabs => LabResults == null || (LabResults.PurityPercent == null && LabResults.GermPercent == null) ? false : true;

        // NO lot number included
        public string CertNumber()
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
                if (CertYear < 2007)
                {
                    return $"{certYearAbbrev}{Variety.Crop.Annual}-{SampleFormCertNumber}";
                }
                return $"{certYearAbbrev}CA-{SampleFormCertNumber}";
            }
            else
            {
                if(CertYear < 2007) {
                    return $"{certYearAbbrev}{Variety.Crop.Annual}-{SampleFormCertNumber}";
                }
                return $"{certYearAbbrev}CA-{SampleFormRad}-{SampleFormCertNumber}";
            }

        }

        public string FullCert() {
            return $"{CertNumber()}-{LotNumber}";
        }

        public string GetCropName()
        {
            if (AppId.HasValue)
            {
                return Application.CropName;
            }
            return Variety == null ? "" : Variety.Crop.Name;
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
