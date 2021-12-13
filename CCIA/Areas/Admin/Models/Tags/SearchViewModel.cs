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
     
    public class AdminTagsSearchViewModel
    {
        public List<ProcessTag> tags { get; set; }             
        
        public string searchTerm { get; set; }

        [Display(Name="Search How")]
        public string searchWhat { get; set; }

        public List<string> searchOptions { get; set; }      

               
        public static async Task<AdminTagsSearchViewModel> Create(CCIAContext _dbContext, AdminTagsSearchViewModel vm, IFullCallService helper)
        {               
            var list = new List<string> { "Tag ID", "SID", "BID", "AppId" , "Tagging Org ID", "Lot Number"}; 
                              
            if(vm != null)
            {
                vm.searchTerm = vm.searchTerm.Trim();
                var p0 = new SqlParameter("@search_term", vm.searchTerm);
                var p1 = new SqlParameter("search_what", System.Data.SqlDbType.VarChar);

                if(vm.searchWhat == "Tag ID")
                {
                    p1.SqlValue = "tag_id";
                }
                if(vm.searchWhat == "SID")
                {
                    p1.SqlValue = "sid";
                }
                 if(vm.searchWhat == "AppId")
                {
                    p1.SqlValue = "appid";
                }
                if(vm.searchWhat == "BID")
                {
                    p1.SqlValue = "bid";
                }
                if(vm.searchWhat == "Tagging Org ID")
                {
                    p1.SqlValue = "conditioner_id";
                }
                if(vm.searchWhat == "Lot Number")
                {
                    p1.SqlValue = "lot_num";
                }
            
                var tagsToFind = await _dbContext.ProcessTag.FromSqlRaw($"EXEC mvc_search_tags_blends @search_term, @search_what", p0, p1).ToListAsync();

                
                
                var viewModel = new AdminTagsSearchViewModel
                {
                    tags = tagsToFind,  
                    searchTerm = vm.searchTerm,
                    searchWhat = vm.searchWhat,
                    searchOptions = list,
                };  
                return viewModel;


            }
            var freshModel = new AdminTagsSearchViewModel
            {
                tags = new List<ProcessTag>(), 
                searchOptions = list,
                searchTerm = "",
                searchWhat = "Tag ID",
            };           

            return freshModel;
        }

        
    } 

    public class AdminTagsSeriesSearchViewModel
    {
        public List<Tags> tags { get; set; }             
        
        public string searchLetter { get; set; }

        public int? searchNumber { get; set; }
               
        public static async Task<AdminTagsSeriesSearchViewModel> Create(CCIAContext _dbContext, AdminTagsSeriesSearchViewModel vm)
        {                     
            if(vm != null)
            {
                var tagsSeriesToFind = _dbContext.TagSeries.Include(ts => ts.Tag).AsQueryable();
                
                if(!string.IsNullOrWhiteSpace(vm.searchLetter))
                {
                    tagsSeriesToFind = tagsSeriesToFind.Where(ts => EF.Functions.Like(ts.Letter, "%" +  vm.searchLetter.Trim() + "%"));
                }
                if(vm.searchNumber != null)
                {
                    tagsSeriesToFind = tagsSeriesToFind.Where(ts => ts.Start <= vm.searchNumber && ts.End >= vm.searchNumber);
                }               
                
                
                var viewModel = new AdminTagsSeriesSearchViewModel
                {
                    tags = await tagsSeriesToFind
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.Variety)                
                        .ThenInclude(v => v.Crop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.ClassProduced)                       
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .ThenInclude(a => a.Variety)                       
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .ThenInclude(a => a.Crop)                        
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend) 
                        .ThenInclude(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                        .ThenInclude(l => l.Seeds)
                        .ThenInclude(s => s.Variety)
                        .ThenInclude(v => v.Crop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => variety
                        .ThenInclude(i => i.Application)
                        .ThenInclude(a => a.Variety)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                        .ThenInclude(i => i.Application) 
                        .ThenInclude(a => a.Crop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                        .ThenInclude(i => i.Crop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                        .ThenInclude(i => i.Variety)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.Blend)
                        .ThenInclude(b => b.Variety) // blendrequest (varietal) => variety => crop
                        .ThenInclude(v => v.Crop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.TagAbbrevClass)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.AbbrevTagType)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.BulkCrop)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.BulkVariety)
                        .Include(ts => ts.Tag)
                        .ThenInclude(t => t.TaggingOrganization)                        
                        .Select(ts => ts.Tag)
                        .ToListAsync(),  
                    searchLetter = vm.searchLetter,
                    searchNumber = vm.searchNumber
                };  
                return viewModel;


            }
            var freshModel = new AdminTagsSeriesSearchViewModel
            {
                tags= new List<Tags>(), 
                searchLetter = "",
                searchNumber = null,
            };           

            return freshModel;
        }

        
    } 
    
}
