using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.ApplicationViewModels
{
    public class NativeSeedSiteEditModel
    {
        public NativeSeedSites Site { get; set; }
        public List<County> Counties { get; set; }


        public static async Task<NativeSeedSiteEditModel> EditModel(CCIAContext _dbContext, int siteId)
        {
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId = 0, Name = "Select County..." });

            var model = new NativeSeedSiteEditModel
            {
                Site = await _dbContext.NativeSeedSites.Where(s => s.Id == siteId).FirstOrDefaultAsync(),
                Counties = counties
            };
            return model;
        }

        public static async Task<NativeSeedSiteEditModel> CreateModel(CCIAContext _dbContext, int appId)
        {
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId = 0, Name = "Select County..." });
            var site = new NativeSeedSites();
            site.AppId = appId;

            var model = new NativeSeedSiteEditModel
            {
                Site = site,
                Counties = counties
            };
            return model;
        }
    }
}
