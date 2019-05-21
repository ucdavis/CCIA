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

        //public String OrgState {get; set; }

        public List<VarOfficial> Varieties {get; set; }

        public bool RenderFormRemainder {get; set; }

        public static async Task<ApplicationViewModel> Create (CCIAContext dbContext, int orgId, int appType) {
            var classToProduce = await dbContext.AbbrevClassProduced.Where(c => c.AppType == appType).ToListAsync();
            var crops = await dbContext.Crops.ToListAsync();
            switch(appType) {
                // Seed
                case 1:
                    crops = await dbContext.Crops.Where(c => c.CertifiedCrop == true).ToListAsync();
                    break;
                // Potato
                case 2:
                    crops = await dbContext.Crops.Where(c => c.Crop == "Potato").ToListAsync();
                    break;
                // HQA
                case 3:
                    crops = await dbContext.Crops.Where(c => c.Heritage == true).ToListAsync();
                    break;
                // PVG
                case 4:
                    crops = await dbContext.Crops.Where(c => c.PreVarietyGermplasm == true).ToListAsync();
                    break;
                // // RQA
                // case 5:
                //     break;
                // // Turfgrass
                // case 6:
                //     break;
                // // Hemp from seed
                // case 7:
                //     break;
                // // Hemp from clones
                // case 8:
                //     break;
            }
            var stateProvince = await dbContext.StateProvince.ToListAsync();
            var organization = await dbContext.Organizations.Where(o => o.OrgId == orgId)
                .Include(o => o.Address)
                .ThenInclude(a => a.Countries)
                .Include(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(o => o.Address)
                .ThenInclude(a => a.County)
                .FirstOrDefaultAsync();           
            /* California's StateProvinceID is 102 -- All applications must come from CA */
            var counties = await dbContext.County
                .Where(c => c.StateProvinceId == 102)
                .ToListAsync();
            var varieties = await dbContext.VarOfficial
                .Select(v => new VarOfficial {
                        VarOffId = v.VarOffId,
                        VarOffName = v.VarOffName,
                        CropId = v.CropId,
                        Crop = v.Crop
                    })
                .ToListAsync();

            var model = new ApplicationViewModel 
            {
                Application = new Applications(),
                ClassProduced = classToProduce,
                Crops = crops,
                StateProvince = stateProvince,
                Counties = counties,
                Organization = organization,
                //OrgState = orgState,
                RenderFormRemainder = false,
                Varieties = varieties
            };

            return model;
        }
    }
}