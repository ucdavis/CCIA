using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using CCIA.Services;

namespace CCIA.Controllers
{
    public class AdminHomeController : AdminController
    {
        private readonly IFileIOService _fileService;

        public AdminHomeController(IFileIOService fileIOService)
        {            
            _fileService = fileIOService;            
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CookieView()
        {
            return View();
        }

        public IActionResult Download(string file)
        {            
            var contentType = "APPLICATION/octet-stream";            
            return File(_fileService.DownloadLibrary(file), contentType, file);
        }
       
    }
}
