using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;

namespace CCIA.Models
{   
     
    public class AdminTagsSearchViewModel
    {
        public List<Tags> tags { get; set; }             
        
        public string searchTerm { get; set; }

        [Display(Name="Search How")]
        public string searchWhat { get; set; }

        public List<string> searchOptions { get; set; }      

               
        public static async Task<AdminTagsSearchViewModel> Create(CCIAContext _dbContext, AdminTagsSearchViewModel vm, IFullCallService helper)
        {               
            var list = new List<string> { "Tag ID", "SID", "BID", "AppId" , "Tagging Org ID", "Lot Number"}; 
                              
            if(vm != null)
            {
                var tagsToFind = helper.FullTag().AsQueryable();

                vm.searchTerm = vm.searchTerm.Trim();
                
                if(vm.searchWhat == "Tag ID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.Id.ToString(), "%" +  vm.searchTerm + "%"));
                }
                if(vm.searchWhat == "SID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.SeedsID.ToString(), '%' + vm.searchTerm + "%"));
                }
                 if(vm.searchWhat == "AppId")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.PotatoAppId.ToString(), '%' + vm.searchTerm + "%"));
                }
                if(vm.searchWhat == "BID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.BlendId.ToString(), '%' + vm.searchTerm + "%"));
                }
                if(vm.searchWhat == "Tagging Org ID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.TaggingOrg.ToString(), '%' + vm.searchTerm + "%"));
                }
                if(vm.searchWhat == "Lot Number")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.Seeds.LotNumber, '%' + vm.searchTerm + "%"));
                }
                
                
                var viewModel = new AdminTagsSearchViewModel
                {
                    tags = await tagsToFind.ToListAsync(),  
                    searchTerm = vm.searchTerm,
                    searchWhat = vm.searchWhat,
                    searchOptions = list,
                };  
                return viewModel;


            }
            var freshModel = new AdminTagsSearchViewModel
            {
                tags = new List<Tags>(), 
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
                    tags = await tagsSeriesToFind.Select(ts => ts.Tag)
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.Variety)                
                        .ThenInclude(v => v.Crop)
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.ClassProduced)                       
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .ThenInclude(a => a.Variety)                       
                        .Include(t => t.Seeds)
                        .ThenInclude(s => s.Application)
                        .ThenInclude(a => a.Crop)                        
                        .Include(t => t.Blend) 
                        .ThenInclude(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                        .ThenInclude(l => l.Seeds)
                        .ThenInclude(s => s.Variety)
                        .ThenInclude(v => v.Crop)
                        .Include(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => variety
                        .ThenInclude(i => i.Application)
                        .ThenInclude(a => a.Variety)
                        .Include(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                        .ThenInclude(i => i.Application) 
                        .ThenInclude(a => a.Crop)
                        .Include(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                        .ThenInclude(i => i.Crop)
                        .Include(t => t.Blend)
                        .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                        .ThenInclude(i => i.Variety)
                        .Include(t => t.Blend)
                        .ThenInclude(b => b.Variety) // blendrequest (varietal) => variety => crop
                        .ThenInclude(v => v.Crop)
                        .Include(t => t.TagAbbrevClass)
                        .Include(t => t.AbbrevTagType)
                        .Include(t => t.BulkCrop)
                        .Include(t => t.BulkVariety)
                        .Include(t => t.TaggingOrganization)                        
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
