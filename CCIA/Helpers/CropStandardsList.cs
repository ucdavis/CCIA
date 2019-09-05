using CCIA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;

namespace CCIA.Helpers
{
    public class CropStandardsList
    {
        private readonly CCIAContext _dbContext;

         public CropStandardsList(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // Type: percent versus count
        
        public bool ShowAssay1 { get; set; }
        public string Assay1Name { get; set; }
        public bool ShowAssay2 { get; set; }
        public string Assay2Name { get; set; }
        public bool ShowDodderGrams { get; set; }

        public bool ShowBeans { get; set; }

        public CropStandardsList()
        {  
            ShowAssay1 = false;
            Assay1Name = "";
            ShowAssay2 = false;
            Assay2Name = "";
            ShowDodderGrams = false;
            ShowBeans = false;  
        }
    }
}