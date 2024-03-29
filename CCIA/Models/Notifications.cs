using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Notifications
    {

        public Notifications()
        {
            Pending = true;
            Created = DateTime.Now;
            AppId = 0;
            SID = 0;
            BlendId = 0;
            TagId = 0;
            OrgId = 0;
            OecdId = 0;
            StId = 0;
            ContactId = 0;
            IsAdmin = false;
            IsWeekly = false;
        }

        public int Id { get; set; }
        
        public string Email { get; set; }
        public int? AppId { get; set; }

        public int? SID { get; set; }

        public int? BlendId { get; set; }

        public int? TagId { get; set; }

        public int? OrgId { get; set; }

        public int? OecdId { get; set; }

        public int? StId { get; set;}
        public int? ContactId { get; set;}

        public string Message { get; set; }

        public bool Pending { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Sent { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWeekly { get; set; }
        
    }
}
