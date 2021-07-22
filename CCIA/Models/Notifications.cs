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
        }

        public int Id { get; set; }
        
        public string Email { get; set; }
        public int? AppId { get; set; }

        public int? SID { get; set; }

        public int? BlendId { get; set; }

        public int? TagId { get; set; }

        public string Message { get; set; }

        public bool Pending { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Sent { get; set; }
        
    }
}
