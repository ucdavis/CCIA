using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Applications
    {
        public Applications()
        {
            InverseTraceNavigation = new HashSet<Applications>();
        }

        public int AppId { get; set; }
        public int? PaperAppNum { get; set; }
        public int? CertNum { get; set; }
        public int? CertYear { get; set; }
        public int? AppOriginalCertYear { get; set; }
        public string LotNo { get; set; }
        
        
        public int? UserAppDataentry { get; set; }
        public int? UserAppModifed { get; set; }
        public DateTime? UserAppModDt { get; set; }
        
        public int? CropId { get; set; }
       
        [Required]
        public string EnteredVariety { get; set; }
        
        public int? ClassProducedAccession { get; set; }
        public DateTime? AppReceived { get; set; }
        public DateTime? AppPostmark { get; set; }
        public DateTime? AppDeadline { get; set; }
        public bool? AppPkgComplete { get; set; }
        public bool? AppSubmitable { get; set; }
        public DateTime? AppCompleteDt { get; set; }
        public string Status { get; set; }
        public bool? Renewal { get; set; }
        public bool? AppApproved { get; set; }
        public string AppApprover { get; set; }
        public DateTime? AppDateAppr { get; set; }
        public int? Trace { get; set; }
        public bool? WarningFlag { get; set; }
        public string ApplicantNotes { get; set; }
        public bool? AppDenied { get; set; }
        public string AppRejector { get; set; }
        public DateTime? AppDateDenied { get; set; }
        public bool? Maps { get; set; }
        public DateTime? MapsSubDt { get; set; }
        public decimal? MapCenterLat { get; set; }
        public decimal? MapCenterLong { get; set; }
        public bool? MapVe { get; set; }
        public string MapUploadFile { get; set; }
        public string TextField { get; set; }
        public string GeoTextField { get; set; }
        public int? MapZoom { get; set; }
        public bool? Tags { get; set; }
        public string PoLotNum { get; set; }
        public int? FieldId { get; set; }
        public string FieldName { get; set; }
       
        
        public DateTime? DatePlanted { get; set; }
        [Required]
        public decimal? AcresApplied { get; set; }
        public bool? Billable { get; set; }
        public bool? Charged { get; set; }
        public string UserEmpModified { get; set; }
        public DateTime? UserEmpDateMod { get; set; }
        public bool? AppCancelled { get; set; }
        public string AppCancelledBy { get; set; }
        public string Comments { get; set; }
        public decimal? AppFee { get; set; }
        public decimal? LateFee { get; set; }
        public decimal? IncompleteFee { get; set; }
        public bool? OverrideLateFee { get; set; }
        public decimal FeeCofactor { get; set; }
        public bool? NotifyNeeded { get; set; }
        public DateTime? NotifyDate { get; set; }
        public DateTime? DateNotified { get; set; }
        public string ApplicantComments { get; set; }
        public string PvgSource { get; set; }
        public string PvgSelectionId { get; set; }
        public string FieldHardiness { get; set; }
        public int? FieldElevation { get; set; }
        public int Ecoregion { get; set; }

       
        public Crops Crop { get; set; }
        public Applications TraceNavigation { get; set; }
        public ICollection<Applications> InverseTraceNavigation { get; set; }

        public Organizations ApplicantOrganization { get; set; }
        public int ApplicantId { get; set; }

        [ForeignKey("GrowerId")]
        public Organizations GrowerOrganization { get; set; }
        public int? GrowerId { get; set; }

        [ForeignKey("FarmCounty")]
        public County County { get; set; }
        public int? FarmCounty { get; set; }

        [ForeignKey("SelectedVarietyId")]
        public VarFull  Variety { get; set; }
        public int? SelectedVarietyId { get; set; }

        [ForeignKey("ClassProducedId")]
        public AbbrevClassProduced ClassProduced { get; set; }
        public int? ClassProducedId { get; set; }

        [ForeignKey("AppType")]
        public AbbrevAppType AppTypeTrans { get; set; }
        public string AppType { get; set; }

        [ForeignKey("AppId")]
        public ICollection<AppCertificates> Certificates { get; set; }

        [ForeignKey("AppId")]
        public ICollection<PlantingStocks> PlantingStocks { get; set; }

        [ForeignKey("AppId")]
        public ICollection<FieldHistory>  FieldHistories { get; set; }

    }
}
