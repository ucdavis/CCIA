using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Seeds
    {
        // public Seeds()
        // {
        //     InverseTraceNavigation = new HashSet<Applications>();
        // }

        public int Id { get; set; }
        public string CertProgram { get; set; } 
        public int? AppId { get; set; }
        public int? SampleFormNumber { get; set; }
        public DateTime? SampleFormDate { get; set; }
        public string SampleFormCertNumber { get; set; }
        public int? SampleFormRad { get; set; }
        public short? CertYear { get; set; }
        
        public Organizations ApplicantOrganization { get; set; }
        public int? ApplicantId { get; set; }

        public Organizations ConditionerOrganization { get; set; }
        public int? ConditionerId { get; set; }

        public int? SampleFormVarietyId { get; set; }
        public int? OfficialVarietyId { get; set; }
        public VarFull  Variety { get; set; }

        public string LotNumber { get; set; }
        public decimal PoundsLot { get; set; }        
        public AbbrevClassSeeds ClassProduced { get; set; }
        public int? Class { get; set; }
        public int? ClassAccession { get; set; }
        public string Status { get; set; }
        public int? CountyDrawn { get; set; }
        public int? OriginState { get; set; }
        public int? OriginCounty { get; set; }
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
        public decimal CropFee { get; set; }
        public decimal CertFee { get; set; }
        public decimal ResearchFee { get; set; }
        public decimal MinimumFee { get; set; }
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
        



        

        

        //public ICollection<FieldHistory>  FieldHistories { get; set; }

    }
}
