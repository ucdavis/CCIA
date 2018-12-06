﻿using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class FieldInspect
    {
        public int FldinspId { get; set; }
        public int? AppId { get; set; }
        public DateTime? DateFldRpt { get; set; }
        public decimal? AcresFld { get; set; }
        public decimal? AcresInspOnly { get; set; }
        public decimal? AcresApproved { get; set; }
        public decimal? AcresCancelled { get; set; }
        public decimal? AcresGrowout { get; set; }
        public decimal? AcresRefund { get; set; }
        public decimal? AcresRejected { get; set; }
        public decimal? AcresNoCrop { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
        public bool? Complete { get; set; }
        public DateTime? DateComplete { get; set; }
        public string CompleteBy { get; set; }
        public bool? Passed { get; set; }
        public int PassClass { get; set; }
        public bool? ReportGenerated { get; set; }
        public DateTime? ReportGenDt { get; set; }
        public bool? Charged { get; set; }
        public string OldFieldName { get; set; }
        public int? OldCountyId { get; set; }
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
