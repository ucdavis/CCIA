using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using CCIA.Services;
using System.IO;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace CCIA.Controllers
{

    
    public class ApiController : Controller
    {
        private readonly CCIAContext _dbContext;
        

        public ApiController(CCIAContext dbContext)
        {
            _dbContext = dbContext;          
        }
        
        public async Task<IActionResult> GetGeoJSON(int year, Guid key)
        {
            var org = await _dbContext.Organizations.Where(o => o.ApiKey == key).FirstOrDefaultAsync();
            StringBuilder geoJson = new StringBuilder("{ \"type\": \"FeatureCollection\",\"features\": [");

            var applicantId = new SqlParameter("applicant_id", org.Id);
            var yearP = new SqlParameter("cert_year", year);
            var geoJsonP = new SqlParameter
            {
                ParameterName = "value",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 2147483647,
            };
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC GetGeoJSONByApplicantAndCertYear_parameter @applicant_id, @cert_year, @value OUTPUT", applicantId, yearP, geoJsonP);
            geoJson.Append(geoJsonP.Value + "]}");            
            return Content(geoJson.ToString());
        }
    }
}