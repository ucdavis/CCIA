using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using NetTopologySuite.Geometries;

namespace CCIA.Models
{
    public partial class Applications
    {

        public Applications()
        {
           PackageComplete = false;
           Submitable = true;
           Status = "Pending supporting material";
           Renewal = false;
           Approved = false;
           Denied = false;
           Maps = false;
           MapVe = false;
           Tags = false;
           Billable = false;
           Charged = false;
           Cancelled = false;
           OverrideLateFee = false;
           NotifyNeeded =false;
           IncompleteFee = 0;
           LateFee = 0;
           Fee = 0;
        }

       
        public int Id { get; set; }
        public int? PaperAppNum { get; set; }
        public int? CertNum { get; set; }

        public int CertYear { get; set; }
        
        [DisplayName("Orig Year")]
        public int? OriginalCertYear { get; set; }
        public string LotNo { get; set; }
        
        
        public int? UserDataentry { get; set; }
        public int? UserAppModifed { get; set; }
        public DateTime? UserAppModDt { get; set; }
         [DisplayName("Entered Variety")]      
        public string EnteredVariety { get; set; }
        
        public int? ClassProducedAccession { get; set; }
        [DisplayName("Entered")]
        public DateTime? Received { get; set; }
        [DisplayName("Submitted")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? Postmark { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        [DisplayName("Complete?")]
        public bool PackageComplete { get; set; }
        public bool Submitable { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Status { get; set; }
        public bool Renewal { get; set; }
        public bool Approved { get; set; }
        public string Approver { get; set; }
        public DateTime? DateApproved { get; set; }
        public int? Trace { get; set; }
        public bool WarningFlag { get; set; }
        public string ApplicantNotes { get; set; }
        public bool Denied { get; set; }
        public string Rejector { get; set; }
        public DateTime? DateDenied { get; set; }
        public bool Maps { get; set; }
        public DateTime? MapsSubmissionDate { get; set; }
        public decimal? MapCenterLat { get; set; }
        public decimal? MapCenterLong { get; set; }
        [DisplayName("Map?")] 
        public bool MapVe { get; set; }
        public string MapUploadFile { get; set; }
        public string TextField { get; set; }
        public string GeoTextField { get; set; }

        public Polygon GeoField { get; set; }
       
        public bool Tags { get; set; }
        public string PoLotNum { get; set; }
        public int? FieldId { get; set; }

        [Required]
        public string FieldName { get; set; }
       
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DatePlanted { get; set; }
        [Required]
        public decimal? AcresApplied { get; set; }
        public bool Billable { get; set; }
        public bool Charged { get; set; }
        public string UserEmpModified { get; set; }
        public DateTime? UserEmpDateMod { get; set; }
        public bool Cancelled { get; set; }
        public string CancelledBy { get; set; }
        public string Comments { get; set; }
        public decimal? Fee { get; set; }
        public decimal? LateFee { get; set; }
        public decimal? IncompleteFee { get; set; }
        public bool OverrideLateFee { get; set; }
        public decimal FeeCofactor { get; set; }
        public bool NotifyNeeded { get; set; }
        public DateTime? NotifyDate { get; set; }
        public DateTime? DateNotified { get; set; }
        public string ApplicantComments { get; set; }
        public string PvgSource { get; set; }
        public string PvgSelectionId { get; set; }
        public string FieldHardiness { get; set; }
        public int? FieldElevation { get; set; }
        public int Ecoregion { get; set; }
       
       [Required]
        public int? CropId { get; set; }

        [ForeignKey("CropId")]
        public Crops Crop { get; set; }
      
        public Organizations ApplicantOrganization { get; set; }
        public int ApplicantId { get; set; }

        [ForeignKey("GrowerId")]
        public Organizations GrowerOrganization { get; set; }

        

        public int? GrowerId { get; set; }

        [ForeignKey("FarmCounty")]
        public County County { get; set; }
        [Required]
        public int? FarmCounty { get; set; }

        public virtual string CountyPermit { get; set; }

        public int? SelectedVarietyId { get; set; }

        [ForeignKey("SelectedVarietyId")]
        public VarFull  Variety { get; set; }

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

        [ForeignKey("AppId")]
        public ICollection<TurfgrassCertificates> TurfgrassCertificates { get; set; }

        [ForeignKey("AppId")]
        public ICollection<FieldResults> FieldResults { get; set; }

        
        [ForeignKey("Id")]
        public FieldInspect FieldInspect { get; set; }
        
        public CertRad AppCertRad {get; set;}

        [ForeignKey("AppId")]
        public ICollection<AppChanges> Changes { get; set; }

        public string CropName 
        { 
            get
            {
                if (CropId == null)
                {
                    return "";
                }
                if(AppType=="PV")
                {
                    return Crop.Name;
                }
                if(Crop != null) 
                {
                    return Crop.Name;

                }
                if (Variety != null && Variety.Crop != null)
                {
                    return Variety.Crop.Name;
                }
                return "";
            } 
        }
        
        public string VarietyName 
        { 
            get
            {
                if(AppType=="PV" || SelectedVarietyId == null)
                {
                    return PvgSelectionId;
                }
                if (Variety != null)
                {
                    return Variety.Name;
                }
                return "";
            } 
        }

        public string QACertNumber
        { 
            get
            {
                string certYearAbbrev = CertYear.ToString().Substring(CertYear.ToString().Length - 2);
                if(AppType=="PV")
                {
                    return $"{certYearAbbrev}CA-PVG-{Id}";
                }
                if(AppType=="GQ")
                {
                    return $"{certYearAbbrev}CA-QA-{Id}";
                }
                if(AppType=="RQ")
                {
                    return $"{certYearAbbrev}CA-RQA-{Id}";
                }               

                return "";

            } 
        }

        public string FullCert 
        { 
            get
            {
                string certYearAbbrev = CertYear.ToString().Substring(CertYear.ToString().Length - 2);
                int rad = -1;
                if(AppCertRad != null){
                    rad = AppCertRad.Rad;
                }
                if(AppType == "PO")
                {
                    return $"{certYearAbbrev}CA-{ApplicantId}-{PoLotNum}";
                }
                if(AppType == "PV")
                {
                    return $"{certYearAbbrev}CA-PVG-{Id}";
                }
                if(AppType=="GQ")
                {
                    return $"{certYearAbbrev}CA-QA-{Id}";
                }
                if(AppType=="RQ")
                {
                    return $"{certYearAbbrev}CA-RQA-{Id}";
                }  
                if(CertYear < 2007)
                {
                    return $"{certYearAbbrev}{Crop.Annual}-{CertNum}";
                }
                return $"{certYearAbbrev}CA-{rad}-{CertNum}";

            }
        }   

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double? AreaAcres {  get; private set; }  
		

        public string GrowerName { 
            get
            {
                if(GrowerOrganization != null)
                {
                    return GrowerOrganization.OrgName;
                }
                return "";
            }
        }

         public string ApplicantName { 
            get
            {
                if(ApplicantOrganization != null)
                {
                    return ApplicantOrganization.OrgName;
                }
                return "";
            }
        }

        public string ClassProducedName { 
            get
            {
                if(ClassProduced != null)
                {
                    return ClassProduced.ClassProducedTrans;
                }
                return ClassProducedId.ToString();
            }
        }

        public bool AppLate { 
            get 
            {
                if(Postmark.HasValue && Deadline.HasValue)
                {
                    if(Postmark.Value.Date > Deadline.Value.Date) 
                    {
                        return true;
                    }
                }                
                return false;
            }
        }

        
        

    }
}
