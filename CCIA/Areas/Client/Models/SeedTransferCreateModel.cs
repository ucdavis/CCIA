using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models.SeedsCreateOOSViewModel;
using CCIA.Models.SeedsCreateViewModel;
using CCIA.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{   
     
    public class SeedTransferRequestModel
    {
        public SeedTransferRequest request { get; set; }  
        public SeedTransfers transfer { get; set; }
               
        public static async Task<SeedTransferRequestModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id, string Target)
        {                   
            var model = new SeedTransferRequestModel();
            
            
            var request = new SeedTransferRequest();
            request.Id = id;
            request.Target = Target;

            if(Target == "SID")
            {
                var seed = await _helper.FullSeeds().Where(s => s.Id == id).FirstOrDefaultAsync(); 
                if(seed == null)
                {
                    return model;
                }    
                request.seed = seed;            
            }
            if(Target == "BID")
            {
                var blend = await _helper.FullBlendRequest().Where(b => b.Id == id).FirstOrDefaultAsync();
                if(blend == null)
                {
                    return model;
                }                
                request.blend = blend;
            }
            if(Target == "AppId")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                if(app == null)
                {
                    return model;
                }                
                request.app = app;
            }            
            model.request = request;  
            model.transfer = new SeedTransfers();                      
            return model;
        }        
    } 

    public class SeedTransferRequest
    {
        public SeedTransferRequest() 
        {
           SubmittedForAnalysis = false;
           CertificateDate = DateTime.Now;
        }


        [Display(Name ="SID/BID/AppID")]
        public int Id { get; set; }
        public string Target { get; set; }
        public Applications app { get; set; }
        public Seeds seed { get; set; }
        public BlendRequests blend { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
        [Display(Name ="Transfer Weight")]
        public Decimal Pounds { get; set; }
        [Display(Name="Planting Stock Lot No.")]
        public string SeedstockLotNumbers { get; set; }
        [Display(Name ="Submitted for Analysis?")]
        public bool SubmittedForAnalysis { get; set; }

        [Display(Name="Certificate Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime CertificateDate { get; set; }
    }   
}
