using System;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Controllers.Admin
{
   
    public class MapController : AdminController
    {
        private readonly CCIAContext _dbContext;

        public MapController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResult> GetFIRSeachObjects(int time,string ids, string geoType)
        {
            var appIds = ids.Split(',').Select(x => int.Parse(x)).ToList();
            var pins = await _dbContext.Applications.Where(a => appIds.Contains(a.Id))
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Select(a => new MapWKT{ Title= "AppId: " + a.Id.ToString(), GeoField = geoType == "point" ? a.GeoField.Centroid.AsText() : a.GeoField.AsText() , GeoType = geoType,  Description = 
                    $"App: {a.ApplicantOrganization.NameAndId}<br>Crp: {a.Crop.Name}<br>Var: {a.Variety.Name}<br>Cl: {a.ClassProduced.ClassProducedTrans}>br>Fld: {a.FieldName}<br>Pt dt: {a.DatePlanted.Value.ToShortDateString()}<br>" +
                    $"<a href='/admin/Application/FieldMap/{a.Id}'>Map</a><br/><a href='admin/Application/Fir/{a.Id}'>FIR</a>"})
                .ToListAsync();
            return Json(pins);
        }             
       
    }
}
