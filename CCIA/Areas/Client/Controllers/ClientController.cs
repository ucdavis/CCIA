using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCIA.Controllers.Client
{
    [Area("Client")]
    [Authorize(Roles = "Member")]  
    public class ClientController : SuperController
    {
    }
}