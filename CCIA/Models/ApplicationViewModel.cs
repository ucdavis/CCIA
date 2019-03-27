using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    public class ApplicationViewModel
    {
        public Applications Application { get; set; }

        public List<AbbrevClassProduced> ClassProduced { get; set; }

        public List<Crops> Crops { get; set; }

        public List<StateProvince> StateProvince {get; set; }

        public List<County> Counties {get; set; }

        public Organizations Organization {get; set; }

        public String OrgState {get; set; }

        // public List<VarOfficial> Varieties { get; set; }

        public static async Task<ApplicationViewModel> Create (CCIAContext dbContext, int orgId) {
            var classToProduce = await dbContext.AbbrevClassProduced.Where(c => c.AppType == 1).ToListAsync();
            var crops = await dbContext.Crops.ToListAsync();
            var stateProvince = await dbContext.StateProvince.ToListAsync();
            var organization = await dbContext.Organizations.Where(o => o.OrgId == orgId)
                .Select(o => new Organizations {
                        OrgId = o.OrgId,
                        OrgName = o.OrgName,
                        Address = o.Address != null ? o.Address : new Address() 
                    })
                .FirstOrDefaultAsync();
            var orgState = organization.Address.StateProvinceId == null ? "" : await dbContext.StateProvince
                .Where(s => s.StateProvinceId == organization.Address.StateProvinceId)
                .Select(s => s.StateProvinceName)
                .FirstOrDefaultAsync();
            var counties = await dbContext.County
                .Where(c => c.StateProvinceId == organization.Address.StateProvinceId)
                .ToListAsync();

            var model = new ApplicationViewModel 
            {
                Application = new Applications(),
                ClassProduced = classToProduce,
                Crops = crops,
                StateProvince = stateProvince,
                Counties = counties,
                Organization = organization,
                OrgState = orgState,
            };

            return model;
        }
    }
}
