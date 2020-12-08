using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;

namespace CCIA.Controllers.Admin
{

    public class TagsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public TagsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public async Task<IActionResult> Process(string stage = "Requested")
        {
            var tag = _helper.FullTag();
            var model = await tag.Where(t => t.Stage == stage).ToListAsync();            
            return View(model);
        }       
    }
}