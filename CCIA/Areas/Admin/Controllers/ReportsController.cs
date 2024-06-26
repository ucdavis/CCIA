using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Controllers.Admin
{
   
    public class ReportsController : AdminController
    {
        private readonly CCIAContext _dbContext;

        

        public ReportsController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Applications(AdminApplicationReportViewModel vm = null)
        {
            var model = await AdminApplicationReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<IActionResult> Seeds(AdminSeedsReportViewModel vm = null)
        {
            var model = await AdminSeedsReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<IActionResult> Rebate(AdminRebateReportViewModel vm = null)
        {
            var model = await AdminRebateReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<IActionResult> OECD(AdminOECDReportViewModel vm = null)
        {
            var model = await AdminOECDReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<IActionResult> Assignments()
        {
            var model = await AdminCropAssignmentReportViewModel.Create(_dbContext);
            return View(model);
        }

        public async Task<IActionResult> Member(AdminMemberReportViewModel vm = null)
        {
            var model = await AdminMemberReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<IActionResult> PotatoBook(AdminPotatoBookReportViewModel vm = null)
        {
            var model = await AdminPotatoBookReportViewModel.Create(_dbContext, vm);
            return View(model);            
        }

        public async Task<IActionResult> BadTags(AdminBadTagsReportViewModel vm = null)
        {
            var model = await AdminBadTagsReportViewModel.Create(_dbContext, vm);
            return View(model);
        }

        

        
        
       
    }
}
