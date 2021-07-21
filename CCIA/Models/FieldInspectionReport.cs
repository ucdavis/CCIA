using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CCIA.Models
{
    public enum FIRPOTestValues
    {
        Negative = 0,
        Positive = 1,
        [Display(Name="Not Completed")]
        NotCompleted = 255
    }    
    public partial class FieldInspectionReport
    {
    
         public FieldInspectionReport()
        {
           Complete = false;
           ReportGenerated = false;
        }

        public int Id { get; set; }

        
        public int AppId { get; set; }    
        [ForeignKey("AppId")]
        public Applications Application { get; set;}    
        public  decimal? AcresInspectionOnly { get; set; }
        public decimal? AcresApproved { get; set; }
        
        [Display(Name="Cancel-Inspected")]
        public decimal? AcresCancelled { get; set; }
        public decimal? AcresGrowout { get; set; }
        
        [Display(Name="Cancel-NoInspect")]
        public decimal? AcresRefund { get; set; }
        public decimal? AcresRejected { get; set; }
        public decimal? AcresNoCrop { get; set; }        
        public bool Complete { get; set; }
       
        public DateTime? DateComplete { get; set; }
        public string CompleteBy { get; set; }
        public int? PassClass { get; set; }

        [ForeignKey("PassClass")]
        public AbbrevClassProduced POPassClass { get; set; }
        
        [Display(Name="FIR Viewed by applicant?")]
        public bool ReportGenerated { get; set; }
        
        [Display(Name="Date FIR Viewed")]
        public DateTime? ReportGenDt { get; set; }
        public string Comments { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? PathDate { get; set; }
        public int? PathNumSamples { get; set; }
        public int? PathNumPlants { get; set; }
        public string PathLab { get; set; }
        public string PathSentVia { get; set; }
        
        [Display(Name="Bacteria (Cms")]
        public int PathCms { get; set; }
        
        [Display(Name="Bateria (Erw")]
        public int PathErw { get; set; }
        [Display(Name="Viroid PSTVd")]
        public int PathPstvd { get; set; }
        [Display(Name="Virus PVA")]
        public int PathPva { get; set; }
        [Display(Name="Virus PVM")]
        public int PathPvm { get; set; }
        [Display(Name="Virus PLRV")]
        public int PathPlrv { get; set; }
        [Display(Name="Virus PVS")]
        public int PathPvs { get; set; }
        [Display(Name="Virus PVX")]
        public int PathPvx { get; set; }
        [Display(Name="Virus PVY")]
        public int PathPvy { get; set; }
        public string PathComments { get; set; }

        [ForeignKey("CompleteBy")]
        public CCIAEmployees CompleteEmployee { get; set; }

        [Display(Name="TotalEntered")]
        public decimal AcresTotalEntered { 
            get
            {                
                return AcresInspectionOnly.GetValueOrDefault() + AcresApproved.GetValueOrDefault() + AcresCancelled.GetValueOrDefault() + AcresGrowout.GetValueOrDefault() + AcresRefund.GetValueOrDefault() + AcresRejected.GetValueOrDefault() + AcresNoCrop.GetValueOrDefault();
            }
        }

        [Display(Name="Marked Complete")]
        public string CompleteText
        { 
            get
            {
                if(!Complete || CompleteEmployee == null)
                {
                    return "";
                }
                return $"on {DateComplete.Value.ToShortDateString()} by {CompleteEmployee.Name}";
            }
        }

        public string PassClassTrans 
        { 
            get 
            {
                if(PassClass == 0)
                {
                    return "As applied";
                }
                if(PassClass == 255)
                {
                    return "Rejected";
                }
                if(POPassClass != null)
                {
                    return POPassClass.ClassProducedTrans;
                }
                return "";
            }
        }

        [Display(Name="Bacteria (Cms)")]
        public FIRPOTestValues PathCmsValue 
        { 
            get
            {
                return (FIRPOTestValues)PathCms;
            }
        }

        [Display(Name="Virus PVA")]
        public FIRPOTestValues PathPvaValue
        {
            get
            {
                return (FIRPOTestValues)PathPva;
            }
        }

        [Display(Name="Virus PVS")]
        public FIRPOTestValues PathPvsValue
        {
            get
            {
                return (FIRPOTestValues)PathPvs;
            }
        }

        [Display(Name="Bacteria (Erw)")]
        public FIRPOTestValues PathErwValue
        {
            get
            {
                return (FIRPOTestValues)PathErw;
            }
        }

        [Display(Name="Virus PVC")]
        public FIRPOTestValues PathPvmValue
        {
            get
            {
                return (FIRPOTestValues)PathPvm;
            }
        }

        [Display(Name="Virus PVX")]
        public FIRPOTestValues PathPvxValue
        {
            get
            {
                return (FIRPOTestValues)PathPvx;
            }
        }

        [Display(Name="Viroid PSTVd")]
        public FIRPOTestValues PathPstvdValue
        {
            get
            {
                return (FIRPOTestValues)PathPstvd;
            }
        }

        [Display(Name="Virus PLRV")]
        public FIRPOTestValues PathPlrvValue
        {
            get
            {
                return (FIRPOTestValues)PathPlrv;
            }
        }

        [Display(Name="Virus PVY")]
        public FIRPOTestValues PathPvyValue
        {
            get
            {
                return (FIRPOTestValues)PathPvy;
            }
        }
    }
}
