using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCIA.Controllers
{
    public class AdminHomeController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CookieView()
        {
            return View();
        }
       
    }
}
