using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CCIA.Models
{       
    public class AdminMapFieldsViewModel
    {
        public List<int> app { get; set; }

        public string mapType { get; set; }

        public AdminMapFieldsViewModel() {
            mapType = "fields";
        }

        public string data { get; set; }

         public static async Task<AdminMapFieldsViewModel> Create(CCIAContext _dbContext, List<string> ids, string requestedType)
        { 
            List<string> strIds = ids.ToString().Split(",").ToList();
            List<int> appIds = ids.Select(s => int.Parse(s.Substring(s.IndexOf("=") + 1, s.Length - s.IndexOf("=")-1))).ToList();            
            var pins = await _dbContext.Applications.Where(a => appIds.Contains(a.Id))
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Select(a => new MapWKT{ AppId = a.Id, Title= "AppId: " + a.Id.ToString(), GeoField = requestedType == "point" ? a.GeoField.Centroid.AsText() : a.GeoField.AsText() , GeoType = requestedType,  Description = 
                    $"App: {a.ApplicantOrganization.NameAndId}<br>Crp: {a.Crop.Name}<br>Var: {a.Variety.Name}<br>Cl: {a.ClassProduced.ClassProducedTrans}>br>Fld: {a.FieldName}<br>Pt dt: {a.DatePlanted.Value.ToShortDateString()}<br>" +
                    $"<a href='/admin/Application/FieldMap/{a.Id}'>Map</a><br/><a href='/admin/Application/Fir/{a.Id}'>FIR</a>"})
                .ToListAsync();
            var test = JsonConvert.SerializeObject(pins);

           

            var viewModal = new AdminMapFieldsViewModel
            {
                app = appIds,
                mapType = requestedType,
                data = test,
            };

            return viewModal;
        }

    }
}