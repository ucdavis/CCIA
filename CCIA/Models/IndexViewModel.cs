using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.IndexViewModels
{
    public class IndexViewModel
    {
        public List<SelectListItem>  certYears { get; set; }  
        public int CertYear { get; set; }      
    }

    public class ApplicationIndexViewModel : IndexViewModel
    {        
        public List<Applications> applications { get; set; }

        public static async Task<ApplicationIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new ApplicationIndexViewModel
            {
                applications = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.ApplicantId == orgId)
                .Include(a => a.GrowerOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .ToListAsync(),
               certYears =  await _dbContext.Applications.Where(a => a.ApplicantId == orgId).Select(a => new SelectListItem() {
                   Text = a.CertYear.ToString(),
                   Value = a.CertYear.ToString()
               }).Distinct().ToListAsync()                
            };

            return viewModel;
        }
    }
}
