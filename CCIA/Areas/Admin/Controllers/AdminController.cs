using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCIA.Controllers
{    
    [Area("Admin")]
    [Authorize(Roles = "Employee")]  
    public class AdminController : SuperController
    {
    }
}