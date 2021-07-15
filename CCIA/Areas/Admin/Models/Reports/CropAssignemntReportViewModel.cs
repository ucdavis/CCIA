using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Models
{   
     
    public class AdminCropAssignmentReportViewModel
    {
       public List<CropAssignmentByLeadBackup> AssignmentByLeadBackup { get; set; }

       public List<CropAssignmentByName> CropAssignmentByName { get; set; }

       public List<CropGroups> CropGroups { get; set; }
        
               
        public static async Task<AdminCropAssignmentReportViewModel> Create(CCIAContext _dbContext)
        {              
            var model = new AdminCropAssignmentReportViewModel
                { 
                    AssignmentByLeadBackup = await _dbContext.CropAssignmentByLeadBackup.FromSqlRaw($"EXEC mvc_crop_assign_by_lead_backup").ToListAsync(),
                    CropAssignmentByName = await _dbContext.CropAssignmentByName.FromSqlRaw($"EXEC mvc_crop_assign_by_name").ToListAsync(),
                    CropGroups = await _dbContext.CropGroups.FromSqlRaw($"EXEC mvc_crop_assign_grouped_crops").ToListAsync(),                    
                };
            

            return model;
        }
        
    }

    public partial class CropAssignmentByLeadBackup
    {
        public string ReportGroup { get; set; }

        public string Lead { get; set; }

        public string Backup1 { get; set; }

        public string Backup2 { get; set; }
        public string Backup3 { get; set; }
        public string Seasonal { get; set; }
    }  

    public partial class CropAssignmentByName
    {
        public string ReportGroup { get; set; }

        public string Blank { get; set; }

        public string Hostert { get; set; }

        public string Koala { get; set; }
        public string Mkandawire { get; set; }
        public string Palmer { get; set; }
    }    

    public partial class CropGroups
    {
        public string ReportGroup { get; set; }

        public int CropId { get; set; }

        public string Crop { get; set; }
    }    
      
    
}
