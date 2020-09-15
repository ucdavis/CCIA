﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CCIA.Models
{
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
        public int PassClass { get; set; }
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
        public int PathCms { get; set; }
        public int PathErw { get; set; }
        public int PathPstvd { get; set; }
        public int PathPva { get; set; }
        public int PathPvm { get; set; }
        public int PathPlrv { get; set; }
        public int PathPvs { get; set; }
        public int PathPvx { get; set; }
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
    }
}
