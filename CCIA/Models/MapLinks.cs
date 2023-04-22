using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Models
{
    public partial class MapLinks
    {
        [Key]
        public int CropptId { get; set; }
        public string Type { get; set; }
        public DateTime DatePlanted { get; set; }
        public string Variety { get; set; }
        public string Status { get; set; }
        public decimal Acres { get; set; }
        public string Description { get; set; }
        public Polygon Field { get; set; }

        public string Data { 
            get {
                var data = new MapWKT { AppId = CropptId, Title = "Croppt: " + CropptId.ToString(), GeoField = Field.AsText(), GeoType = "Polygon", Description =  
                    $"Var: {Variety}<br>Status: {Status}<br>Type: {Type}<br>Pt dt: {DatePlanted.ToShortDateString()}"};
                List<MapWKT> list = new List<MapWKT>();
                list.Add(data);
                return  JsonConvert.SerializeObject(list);
            }
        }

    }   
}