using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class FieldInspectionReport
    {
        
        public int AppId { get; set; }        
        public  decimal? AcresInspOnly { get; set; }
        public decimal? AcresApproved { get; set; }
        public decimal? AcresCancelled { get; set; }
        public decimal? AcresGrowout { get; set; }
        public decimal? AcresRefund { get; set; }
        public decimal? AcresRejected { get; set; }
        public decimal? AcresNoCrop { get; set; }        
        public bool? Complete { get; set; }
        public DateTime? DateComplete { get; set; }
        public string CompleteBy { get; set; }
        public int PassClass { get; set; }
        public bool? ReportGenerated { get; set; }
        public DateTime? ReportGenDt { get; set; }
        public string FldInspectComments { get; set; }
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
    }
}
