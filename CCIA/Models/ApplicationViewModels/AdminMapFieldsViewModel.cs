using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using CCIA.Models.DetailsViewModels;
using CCIA.Services;

namespace CCIA.Models
{       
    public class AdminMapFieldsViewModel
    {
        public List<int> app { get; set; }
        
        public AdminViewModel details { get; set; }

        public string data { get; set; }

         public static async Task<AdminMapFieldsViewModel> Create(CCIAContext _dbContext, List<string> ids, string requestedType)
        { 
            List<string> strIds = ids.ToString().Split(",").ToList();
            List<int> appIds = ids.Select(s => int.Parse(s.Substring(s.IndexOf("=") + 1, s.Length - s.IndexOf("=")-1))).ToList();            
            var pins = await _dbContext.Applications.Where(a => appIds.Contains(a.Id) && a.MapVe)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Select(a => new MapWKT{ AppId = a.Id, Title= "AppId: " + a.Id.ToString(), GeoField = requestedType == "point" ? a.GeoField.Centroid.AsText() : a.GeoField.AsText() , GeoType = requestedType,  Description = 
                    $"App: {a.ApplicantOrganization.NameAndId}<br>Crp: {a.Crop.Name}<br>Var: {a.Variety.Name}<br>Cl: {a.ClassProduced.ClassProducedTrans}>br>Fld: {a.FieldName}<br>Pt dt: {a.DatePlanted.Value.ToShortDateString()}<br>" +
                    $"<a href='/admin/Application/FieldMap/{a.Id}'>Map</a><br/><a href='/admin/Application/Fir/{a.Id}'>FIR</a>"})
                .ToListAsync();

            var viewModal = new AdminMapFieldsViewModel
            {
                app = appIds,
                data = JsonConvert.SerializeObject(pins),
            };

            return viewModal;
        }

         public static async Task<AdminMapFieldsViewModel> SingleMap(CCIAContext _dbContext, int appId, IFullCallService _helper)
        { 
                       
            var pins = await _dbContext.Applications.Where(a => a.Id == appId & a.MapVe)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Select(a => new MapWKT{ AppId = a.Id, Title= "AppId: " + a.Id.ToString(), GeoField = a.GeoField.AsText() , GeoType = "Polygon",  Description = 
                    $"App: {a.ApplicantOrganization.NameAndId}<br>Crp: {a.Crop.Name}<br>Var: {a.Variety.Name}<br>Cl: {a.ClassProduced.ClassProducedTrans}<br>Fld: {a.FieldName}<br>Pt dt: {a.DatePlanted.Value.ToShortDateString()}<br>" +
                    $"<a href='/admin/Application/FieldMap/{a.Id}'>Map</a><br/><a href='/admin/Application/Fir/{a.Id}'>FIR</a>"})
                .ToListAsync();           

            var viewModal = new AdminMapFieldsViewModel
            {
                app = new List<int> {appId},
                data = JsonConvert.SerializeObject(pins),
                details = await AdminViewModel.CreateDetails(_dbContext, appId, _helper),
            };

            return viewModal;
        }

    }
}