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

        public static async Task<ApplicationViewModel> Create (CCIAContext dbContext) {
            var classToProduce = await dbContext.AbbrevClassProduced.Where(c => c.AppType == 1).ToListAsync();
            var crops = await dbContext.Crops.ToListAsync();
            var stateProvince = await dbContext.StateProvince.ToListAsync();
            var counties = await dbContext.County.ToListAsync();

            var model = new ApplicationViewModel 
            {
                Application = new Applications(),
                ClassProduced = classToProduce,
                Crops = crops,
                StateProvince = stateProvince,
                Counties = counties
            };

            return model;

        }
    }
}
