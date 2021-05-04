using System;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Controllers.Admin
{
   
    public class BlendsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public BlendsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

       public async Task<IActionResult> Process()
       {
           var model = await _helper.FullBlendRequest().Where(b => b.Status == BlendStatus.PendingAcceptance.GetDisplayName()).ToListAsync();
           return View(model);
       }  

       public async Task<IActionResult> Details(int id)
       {
           var model = await AdminBlendsDetailsViewModel.Create(_dbContext, _helper, id);           
           if(model.blend == null)
           {
               ErrorMessage = "Blend Request not found";
                RedirectToAction(nameof(Process));
           }
           return View(model);
       }         
       
    }
}
