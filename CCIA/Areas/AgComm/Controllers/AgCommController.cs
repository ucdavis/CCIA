using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCIA.Controllers.AgComm
{    
    [Area("AgComm")]
    [Authorize(Roles = "AgComm")]  
    public class AgCommController : SuperController
    {
    }
}