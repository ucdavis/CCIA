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

        [DisplayName("Search How")]
        public string searchWhat { get; set; }

        public List<string> searchOptions { get; set; }      

               
        public static async Task<AdminTagsSearchViewModel> Create(CCIAContext _dbContext, AdminTagsSearchViewModel vm, IFullCallService helper)
        {   
            // var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeId).ToListAsync();
            // appTypes.Add(new AbbrevAppType { AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "Any"});
            var list = new List<string> { "Tag ID", "SID", "BID" , "Tagging Org ID", "Lot Number"}; 
                              
            if(vm != null)
            {
                var tagsToFind = helper.FullTag().AsQueryable();
                
                if(vm.searchWhat == "Tag ID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.Id.ToString(), "%" +  vm.searchTerm + "%"));
                }
                if(vm.searchWhat == "SID")
                {
                    tagsToFind = tagsToFind.Where(t => EF.Functions.Like(t.SeedsID.ToString(), '%' + vm.searchTerm + "%"));
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
    
}
