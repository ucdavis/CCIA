using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Thinktecture;

namespace CCIA.Models.ViewModels
{   
    public class WeightLogViewModel 
    {

        public List<WeightLog> seeds { get; set; }
        public List<int> Years { get; set; }
        public int Year { get; set; }

        public static async Task<WeightLogViewModel> Create(CCIAContext _dbContext, int year)
        {             
            var viewModel = new WeightLogViewModel
            {
                seeds = _dbContext.Seeds.Where(s => s.YearConfirmed == year && s.Submitted)                    
                    .Include(c => c.ConditionerOrganization)
                    .Include(c => c.AppTypeTrans)
                    .Include(v => v.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(c => c.ClassProduced)
                    .Include(l => l.LabResults) 
                    .ThenInclude(l => l.LabOrganization)                   
                    .Include(s => s.Application)
                    .ThenInclude(a => a.Crop)
                    .Include(s => s.OECDForm)
                    .Select(s => new WeightLog
                    {
                        Id = s.Id,
                        ConditionerId = s.ConditionerId, DateSampleReceived = s.DateSampleReceived,
                        ConfirmedAt = s.ConfirmedAt, SampleFormDate = s.SampleFormDate,
                        OECDLot = s.OECDLot, CertYear = s.CertYear, LotNumber = s.LotNumber,
                        PoundsLot = s.PoundsLot, ConditionerName = s.ConditionerOrganization.Name,
                        Class = s.Class, ClassName = s.ClassProduced.Abbrv, 
                        OutsideCalifornia = s.OriginState == 102, AppId = s.AppId, 
                        SampleFormCertNumber = s.SampleFormCertNumber, CropAnnual = s.Variety.Crop.Annual,
                        SampleFormRad = s.SampleFormRad, CropName = s.Variety.Crop.Name,
                        VarietyName = s.Variety.Name, PrivateLabDate = s.LabResults.PrivateLabDate,
                        PrivateLabNumber = s.LabResults.PrivateLabNumber,
                        LabName = s.LabResults.LabOrganization == null ? s.LabResults.PrivateLabName : s.LabResults.LabOrganization.Name,
                        Rejected = s.LabResults.PurityResults == "R" || s.LabResults.GermResults == "R" || s.LabResults.AssayResults == "R" ? true : false,
                        HasOECDForm = s.OECDForm.Any()
                        
                    }
                
                    ).AsEnumerable()
                    .Select((u, index) => new WeightLog { Id = u.Id, ConditionerId = u.ConditionerId, DateSampleReceived = u.DateSampleReceived, 
                        ConfirmedAt = u.ConfirmedAt, SampleFormDate = u.SampleFormDate, OECDLot = u.OECDLot, CertYear = u.CertYear, LotNumber = u.LotNumber,
                        PoundsLot = u.PoundsLot, ConditionerName = u.ConditionerName, Class = u.Class, ClassName = u.ClassName, OutsideCalifornia = u.OutsideCalifornia, AppId = u.AppId, 
                        SampleFormCertNumber = u.SampleFormCertNumber, CropAnnual = u.CropAnnual, SampleFormRad = u.SampleFormRad, CropName = u.CropName,
                        VarietyName = u.VarietyName, PrivateLabDate = u.PrivateLabDate, PrivateLabNumber = u.PrivateLabNumber, LabName = u.LabName,
                        Rejected = u.Rejected, HasOECDForm = u.HasOECDForm, RowNumber = index + 1} )
                    .ToList(),
                Years = await _dbContext.Seeds.Select(s => s.YearConfirmed).Distinct().OrderBy(id => id).ToListAsync(),
                Year = year,                
            };

            return viewModel;
        }
    }    
}