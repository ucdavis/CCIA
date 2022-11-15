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
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
               
        public static async Task<SeedTransferRequestModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id, string Target, int OrgId)
        {                   
            var model = new SeedTransferRequestModel();
            model.Error = false;
            var customers = await _dbContext.MyCustomers.Where(c => c.OrganizationId == OrgId).ToListAsync();
            customers.Insert(0, new MyCustomers { Id=0, Name="Select customer..."});
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId=0, Name="Select county..."});


            var p0 = new SqlParameter("@id", id);
            var p1 = new SqlParameter("@class_type", Target);
            
            var request = new SeedTransferRequest
            {
                Id = id,
                Target = Target,
                states =  await _dbContext.StateProvince.Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),               
                countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                customers = customers,
                counties = counties,            
                classes = await _dbContext.AbbrevClassSeeds.FromSqlRaw($"EXEC mvc_class_producable_from_id @id, @class_type", p0, p1).ToListAsync(),
            };
            
            model.transfer = new SeedTransfers();     
            model.transfer.CertificateDate = DateTime.Now;
            model.transfer.SubmittedForAnalysis = false;            

            if(Target == "SID")
            {
                var seed = await _helper.FullSeeds().Where(s => s.Id == id && (s.ConditionerId == OrgId || s.ApplicantId == OrgId)).FirstOrDefaultAsync(); 
                if(seed == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "SID not found or does not belong to your company.";
                    return model;
                }    
                request.seed = seed;            
            }
            if(Target == "BID")
            {
                var blend = await _helper.FullBlendRequest().Where(b => b.Id == id && b.ConditionerId == OrgId).FirstOrDefaultAsync();
                if(blend == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "Blend not found or does not belong to your company.";
                    return model;
                }                
                request.blend = blend;
            }
            if(Target == "AppId")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id && a.ApplicantId == OrgId).FirstOrDefaultAsync();
                if(app == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "AppId not found or does not belong to your company.";
                    return model;
                }                
                request.app = app;
                model.transfer.StageFromFieldNumberOfAcres = app.AcresApplied.Value;
            }            
            model.request = request;  
                 
                       
            return model;
        }   

        public static async Task<SeedTransferRequestModel> Retry(CCIAContext _dbContext, IFullCallService _helper , int id, string Target, int OrgId, SeedTransfers transfer)
        {                   
            var model = new SeedTransferRequestModel();
            model.Error = false;
            var customers = await _dbContext.MyCustomers.Where(c => c.OrganizationId == OrgId).ToListAsync();
            customers.Insert(0, new MyCustomers { Id=0, Name="Select customer..."});
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId=0, Name="Select county..."});


            var p0 = new SqlParameter("@id", id);
            var p1 = new SqlParameter("@class_type", Target);
            
            var request = new SeedTransferRequest
            {
                Id = id,
                Target = Target,
                states =  await _dbContext.StateProvince.Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),               
                countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                customers = customers,
                counties = counties,
                classes = await _dbContext.AbbrevClassSeeds.FromSqlRaw($"EXEC mvc_class_producable_from_id @id, @class_type", p0, p1).ToListAsync(),
            };
            

            if(Target == "SID")
            {
                var seed = await _helper.FullSeeds().Where(s => s.Id == id).FirstOrDefaultAsync(); 
                if(seed == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "SID not found or does not belong to your company.";
                    return model;
                }    
                request.seed = seed;            
            }
            if(Target == "BID")
            {
                var blend = await _helper.FullBlendRequest().Where(b => b.Id == id).FirstOrDefaultAsync();
                if(blend == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "Blend not found or does not belong to your company.";
                    return model;
                }                
                request.blend = blend;
            }
            if(Target == "AppId")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                if(app == null)
                {
                    model.Error = true;
                    model.ErrorMessage = "AppId not found or does not belong to your company.";
                    return model;
                }                
                request.app = app;
            }            
            model.request = request;  
            model.transfer = transfer;             
            return model;
        }      
    } 

    public class SeedTransferRequest
    {
        [Display(Name ="SID/BID/AppID")]
        public int Id { get; set; }
        public string Target { get; set; }
        public Applications app { get; set; }
        public Seeds seed { get; set; }
        public BlendRequests blend { get; set; }
        public List<MyCustomers> customers { get; set; }
        public List<County> counties { get; set; }
        public List<StateProvince> states { get; set; }
        public List<Countries> countries { get; set; }
        public List<AbbrevClassSeeds> classes { get; set; }
        public int? OrgId { get; set; }


        
    }   
}
