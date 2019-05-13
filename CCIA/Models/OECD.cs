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

        public int? VarietyId { get; set; }

        public int? TagId { get; set; }

        public int? Pounds { get; set; }

        public string CertNumber { get; set; }

        public string OECDNumber { get; set; }

        public int? Class { get; set; }

        public DateTime? CloseDate { get; set; }

        public int? ConditionerId { get; set; }

        public int? Country { get; set; }

        public DateTime? IssueDate { get; set; }

        public string LotNumber { get; set; }

        public int? ShipperId { get; set; }

        public DateTime? DateRequested { get; set; }

        public decimal? TotalFee { get; set; }

        public bool NotCertified { get; set; }

        public DateTime? DataEntryDate { get; set; }

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

        public string DataEntryUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public bool DomesticOrigin { get; set; }

        public bool Canceled { get; set; }

        public string Comments { get; set; }

        public string AdminComments { get; set; }

        public DateTime? DatePrinted { get; set; }

        public string ReferenceNumber { get; set; }

        public bool USDAReported { get; set; }

        public DateTime? USDAReportDate { get; set; }

        public int TagsRequested { get; set; }

        public decimal? CertificateFee { get; set; }

        public decimal? OECDFee { get; set; }

        public decimal? NotFinallyCertifiedFee { get; set; }

        public bool ClientNotified { get; set; }

    }
}