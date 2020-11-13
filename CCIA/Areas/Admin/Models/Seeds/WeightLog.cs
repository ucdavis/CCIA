using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{

     public class WeightLog
    {         
        public int Id { get; set; }
        
        public string CertProgram { get; set; }
        
        [Display(Name = "Sample Form Date")] 
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SampleFormDate { get; set; }
        
        public bool HasOECDForm { get; set; }
         public string SampleFormCertNumber { get; set; }
        
        public int? CertYear { get; set; }

        public DateTime? DateSampleReceived { get; set; }

        public string ConditionerName { get; set; }
        public int ConditionerId { get; set; }

        public string Variety { get; set; }        

        [Display(Name="Lot Number")]
        public string LotNumber { get; set; }
        [Display(Name="Lot Weight (pounds)")]
        public decimal PoundsLot { get; set; }

        public int? Class { get; set; }
        public string ClassName { get; set; }
        
        [Display(Name="OECD Lot?")]
        public bool OECDLot { get; set; }
        public DateTime? ConfirmedAt { get; set; }        
        
        public int YearConfirmed { get; set; }        
       
        public long RowNumber { get; set; }
        public string CropName { get; set; }
        public string VarietyName { get; set; }
        public string LabName { get; set; }

        public DateTime? PrivateLabDate { get; set; }
        public string PrivateLabNumber { get; set; }

        public bool Rejected { get; set; }

        public bool OutsideCalifornia { get; set; }

        public int? AppId { get; set; }
        public string CropAnnual { get; set; }
        public int? SampleFormRad { get; set; }

        public string CertNumber
        {
            get
            {
                string certYearAbbrev = CertYear.ToString().Substring(CertYear.ToString().Length - 2);
                if (OutsideCalifornia || AppId != null)
                {
                    if (AppId != null)
                    {
                        return SampleFormCertNumber;
                    }
                    if (OutsideCalifornia)
                    {
                        return SampleFormCertNumber;
                    }
                    if (CertYear < 2007)
                    {
                        return $"{certYearAbbrev}{CropAnnual}-{SampleFormCertNumber}";
                    }
                    return $"{certYearAbbrev}CA-{SampleFormCertNumber}";
                }
                else
                {
                    if (CertYear < 2007 || SampleFormRad == null)
                    {
                        return $"{certYearAbbrev}{CropAnnual}-{SampleFormCertNumber}";
                    }                   
                    return $"{certYearAbbrev}CA-{SampleFormRad}-{SampleFormCertNumber}";
                }
            }
        }

       
    }
}