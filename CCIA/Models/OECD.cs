using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class OECD
    {
        public int Id { get; set; }

        public int? SeedsId { get; set; }

        [ForeignKey("SeedsId")]
        public Seeds Seeds { get; set; }

        public int? BID { get; set; }

        [ForeignKey("BID")]
        public BlendRequests Blend { get; set; }

        public int? VarietyId { get; set; }
        [ForeignKey("VarietyId")]
        public VarFull Variety { get; set; }

        public int? TagId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.##}")]
        public decimal? Pounds { get; set; }

        [Display(Name ="Cert #")]
        public string CertNumber { get; set; }

        public string OECDNumber { get; set; }

        public int? ClassId { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevOECDClass Class { get; set; }


         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
         [Display(Name ="Close Date")]
        public DateTime? CloseDate { get; set; }

        public int? ConditionerId { get; set; }

        [ForeignKey("ConditionerId")]
        public Organizations ConditionerOrganization { get; set; }

        public int? CountryId { get; set; }

        public Countries Country { get; set; }

        public DateTime? IssueDate { get; set; }

        [Display(Name ="Lot #")]
        public string LotNumber { get; set; }

        public int? ShipperId { get; set; }
        [ForeignKey("ShipperId")]
        public Organizations ShipperOrganization { get; set; }

        [Display(Name ="Requested")]
        public DateTime? DateRequested { get; set; }

        [Display(Name ="Total Fee")]
        public decimal? TotalFee { get; set; }

        [Display(Name ="Not Certified")]
        public bool NotCertified { get; set; }

        [Display(Name ="Entered")]
        public DateTime? DataEntryDate { get; set; }

        public bool FollowUp { get; set; }

        public int DataEntryYear
        {
            get
            {
                if (DataEntryDate.HasValue)
                {
                    if (DataEntryDate.Value.Date.Month == 10 || DataEntryDate.Value.Date.Month == 11 || DataEntryDate.Value.Date.Month == 12)
                    {
                        return DataEntryDate.Value.Date.Year + 1;
                    }
                    else
                    {
                        return DataEntryDate.Value.Date.Year;
                    }
                }
                return 0;
            }
        }

        [Display(Name ="Entered By")]
        public string DataEntryUser { get; set; }

        [ForeignKey("DataEntryUser")]
        public CCIAEmployees EntryEmployee { get; set; }

        [Display(Name ="Updated")]
        public DateTime? UpdateDate { get; set; }

        [Display(Name ="Updated By")]
        public string UpdateUser { get; set; }
        [ForeignKey("UpdateUser")]
        public CCIAEmployees UpdateEmployee { get; set; }

        [Display(Name ="Domestic Origin?")]
        public bool DomesticOrigin { get; set; }

        public bool Canceled { get; set; }

        public string Comments { get; set; }

        [Display(Name ="Admin Comments")]
        public string AdminComments { get; set; }

        [Display(Name ="Printed")]
        public DateTime? DatePrinted { get; set; }

        public int FiscalYearPrinted 
        { 
            get
            {
                if(!DatePrinted.HasValue)
                {
                    return 0;
                }
                if(DatePrinted.Value.Month == 7 || DatePrinted.Value.Month == 8 || DatePrinted.Value.Month == 9 || DatePrinted.Value.Month == 10 || DatePrinted.Value.Month == 11 || DatePrinted.Value.Month == 12)
                {
                    return DatePrinted.Value.Year;
                }
                return DatePrinted.Value.Year - 1;

            }
        }

        public string ReferenceNumber { get; set; }

        public bool USDAReported { get; set; }

        public DateTime? USDAReportDate { get; set; }

        [Display(Name ="Tags Requested")]
        public int TagsRequested { get; set; }

        public decimal? CertificateFee { get; set; }

        public decimal? OECDFee { get; set; }

        public decimal? NotFinallyCertifiedFee { get; set; }

        public bool ClientNotified { get; set; }

        [ForeignKey("OECDId")]
        public ICollection<OECDChanges> Changes { get; set; }

        public string USDACertNumber { 
            get
            {
                if(Seeds == null){
                    return "";
                }
                return $"CA{Seeds.CertYear}-{Id.ToString("D6")}";

            }
        }

        public int LinkId { 
            get
            {
                if(SeedsId != null)
                {
                    return SeedsId.Value;
                }
                if(BID != null)
                {
                    return BID.Value;
                }
                return 0;
            }
        }

        public string LinkTitle {
            get
            {
                if(SeedsId != null)
                {
                    return "SID";
                }
                if(BID != null)
                {
                    return "Blend ID";
                }
                return "Unknown";

            }
         }

        public string LinkClass {
            get
            {
                if(Seeds != null && Seeds.ClassProduced != null)
                {
                    return Seeds.ClassProduced.CertClass;
                }
                if(Blend != null)
                {
                    return "Blend";
                }
                return "Unknown";
            }
        }

        public int CertYear { 
            get
            {
                if(Seeds != null)
                {
                    return Seeds.CertYear.Value;
                }
                if(Blend != null)
                {
                    return Blend.CertYear;
                }
                return 0;
            }
        }

        public string Flag
        {
            get
            {
                if(BID.HasValue)
                {
                    return "Blend used. Please manually check 6 month reqruirment on lab results.";
                }
                if(Seeds != null && Seeds.LabResults != null && Seeds.LabResults.PrivateLabDate.HasValue && CloseDate.HasValue)
                {
                    
                    if(Seeds.LabResults.PrivateLabDate.Value.AddMonths(6) <= CloseDate.Value)
                    {
                        return "Warning: It appears the close date is more than 6 months from the lab results on the SID.";
                    }
                    if(Seeds.LabResults.PrivateLabDate.Value > CloseDate.Value)
                    {
                        return "Lab results lab date is greater than the close date. Please double check this.";
                    }
                }
                if(Seeds!= null && Seeds.LabResults != null && !Seeds.LabResults.PrivateLabDate.HasValue)
                {
                    return "Alert: Lab results does not have a date (can't check the 6 months requirement)";
                }
                if(Seeds == null || Seeds.LabResults == null)
                {
                    return "No Seed or Lab Results provided (can't check 6 month requirement)";
                }
                return "";
            }
        }

       

        [Display(Name="USDA Ref#")]
        public string LotReferenceNumber { 
            get
            {
                if(!string.IsNullOrWhiteSpace(OECDNumber))
                {
                    return OECDNumber;
                }
                if(Seeds == null)
                {
                    return "";
                }
                string certYearAbbrev = Seeds.CertYear.ToString().Substring(Seeds.CertYear.ToString().Length - 2);
                var fileList = new List<int>(new int[] {20185,20184,19858,19796,19797,19798,19799,19800,19801,19802,19803,19804,19805,19806,19807,19808,19809,19810,19811,19812,19813,19814,19815,19816,19817,19818,19819,19820,19821,19822,19823,19824,19825,19826,19827,19828,19829,19830,19832,19844,19845,19846,19847,19848,19849,19850,19852,19853,19854,19855,19837,19838,19839,19840,19841,19842,19837,19838,19839,19840,19841,19842,19843,19849,19860});
                if(fileList.Contains(Id))
                {
                    return $"USA{certYearAbbrev}CA-{CertNumber}-{LotNumber}";
                }
                if(DataEntryDate > DateTime.Parse("7/1/10") )
                {
                    return $"USA-CA-{certYearAbbrev}CA-{CertNumber}-{LotNumber}";
                }
                if(DataEntryDate > DateTime.Parse("9/1/09") )
                {
                    return $"USACA-{certYearAbbrev}CA-{CertNumber}-{LotNumber}";
                }
                if(DataEntryDate > DateTime.Parse("4/30/09") )
                {
                    return $"USA{certYearAbbrev}CA-{CertNumber}-{LotNumber}";
                }
                if(DomesticOrigin && Seeds.CertYear < 2007)
                {
                    return $"USA (CA) {certYearAbbrev}{Seeds.Variety.Crop.Annual}-{CertNumber}{LotNumber}";
                }
                if(DomesticOrigin)
                {
                    return $"USA (CA) {certYearAbbrev}CA-{CertNumber}-{LotNumber}";
                }


                return $"{CertNumber}={LotNumber}";


            } 
        }

    }
}