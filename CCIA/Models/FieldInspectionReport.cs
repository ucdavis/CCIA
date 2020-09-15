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

        
        public int AppId { get; set; }        
        public  decimal? AcresInspectionOnly { get; set; }
        public decimal? AcresApproved { get; set; }
        [DisplayName("Cancel-Inspected")]
        public decimal? AcresCancelled { get; set; }
        public decimal? AcresGrowout { get; set; }
        [DisplayName("Cancel-NoInspect")]
        public decimal? AcresRefund { get; set; }
        public decimal? AcresRejected { get; set; }
        public decimal? AcresNoCrop { get; set; }        
        public bool Complete { get; set; }
       
        public DateTime? DateComplete { get; set; }
        public string CompleteBy { get; set; }
        public int? PassClass { get; set; }

        [ForeignKey("PassClass")]
        public AbbrevClassProduced POPassClass { get; set; }
        [DisplayName("FIR Viewed by applicant?")]
        public bool ReportGenerated { get; set; }
        [DisplayName("Date FIR Viewed")]
        public DateTime? ReportGenDt { get; set; }
        public string Comments { get; set; }
        public DateTime? PathDate { get; set; }
        public int? PathNumSamples { get; set; }
        public int? PathNumPlants { get; set; }
        public string PathLab { get; set; }
        public string PathSentVia { get; set; }
        [DisplayName("Bacteria (Cms)")]
        public int PathCms { get; set; }
        [DisplayName("Bacteria (Erw)")]
        public int PathErw { get; set; }
        [DisplayName("Viroid PSTVd")]
        public int PathPstvd { get; set; }
        [DisplayName("Virus PVA")]
        public int PathPva { get; set; }
        [DisplayName("Virus PVM")]
        public int PathPvm { get; set; }
        [DisplayName("Virus PLRV")]
        public int PathPlrv { get; set; }
        [DisplayName("Virus PVS")]
        public int PathPvs { get; set; }
        [DisplayName("Virus PVX")]
        public int PathPvx { get; set; }
        [DisplayName("Virus PVY")]
        public int PathPvy { get; set; }
        public string PathComments { get; set; }

        [ForeignKey("CompleteBy")]
        public CCIAEmployees CompleteEmployee { get; set; }

        [DisplayName("Total Entered")]
        public decimal AcresTotalEntered { 
            get
            {                
                return AcresInspectionOnly.GetValueOrDefault() + AcresApproved.GetValueOrDefault() + AcresCancelled.GetValueOrDefault() + AcresGrowout.GetValueOrDefault() + AcresRefund.GetValueOrDefault() + AcresRejected.GetValueOrDefault() + AcresNoCrop.GetValueOrDefault();
            }
        }

        [DisplayName("Marked Complete")]
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

        [DisplayName("Bacteria (Cms)")]
        public FIRPOTestValues PathCmsValue 
        { 
            get
            {
                return (FIRPOTestValues)PathCms;
            }
        }

        [DisplayName("Virus PVA")]
        public FIRPOTestValues PathPvaValue
        {
            get
            {
                return (FIRPOTestValues)PathPva;
            }
        }

        [DisplayName("Virus PVS")]
        public FIRPOTestValues PathPvsValue
        {
            get
            {
                return (FIRPOTestValues)PathPvs;
            }
        }

        [DisplayName("Bacteria (Erw)")]
        public FIRPOTestValues PathErwValue
        {
            get
            {
                return (FIRPOTestValues)PathErw;
            }
        }

        [DisplayName("Virus PVM")]
        public FIRPOTestValues PathPvmValue
        {
            get
            {
                return (FIRPOTestValues)PathPvm;
            }
        }

        [DisplayName("Virus PVX")]
        public FIRPOTestValues PathPvxValue
        {
            get
            {
                return (FIRPOTestValues)PathPvx;
            }
        }

        [DisplayName("Viroid PSTVd")]
        public FIRPOTestValues PathPstvdValue
        {
            get
            {
                return (FIRPOTestValues)PathPstvd;
            }
        }

        [DisplayName("Virus PLRV")]
        public FIRPOTestValues PathPlrvValue
        {
            get
            {
                return (FIRPOTestValues)PathPlrv;
            }
        }

        [DisplayName("Virus PVY")]
        public FIRPOTestValues PathPvyValue
        {
            get
            {
                return (FIRPOTestValues)PathPvy;
            }
        }
    }
}
