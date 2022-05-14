using CCIA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Helpers
{
    public class SeedTransferValidator
    {
        public bool HasWarnings { get; set; }
        public string Error { get; set; }
       


        public SeedTransferValidator()
        {
            HasWarnings = false;
            Error = "";
        }

        public static SeedTransferValidator CheckStandards(SeedTransfers stc)
        {
            var returnList = new SeedTransferValidator();

            // TODO: check county vs state.
            
            if(stc.StageInDirt && stc.StageConditioned)
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Transfer must be either in-dirt or conditioned, not both; ";
            }
            if(stc.StageInDirt && !(stc.StageFromField || stc.StageFromStorage))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "In-dirt samples must be from either field or storage; ";
            }
            if(stc.StageFromField && stc.StageFromStorage)
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "In-dirt samples must either be from field or storage, not both; ";
            }
            if(stc.StageInDirt && (stc.StageNotFinallyCertified || stc.StageCertifiedSeed))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If in-dirt, can not select any conditioned status; ";
            }
            if(stc.StageFromField && string.IsNullOrWhiteSpace(stc.StageFromFieldNumberOfAcres.ToString()))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If you select from field, must enter number of acres; ";
            }
            if(stc.StageConditioned && (stc.StageFromField || stc.StageFromStorage))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If you select conditioned, you can not also select from field or storage; ";
            }
            if(stc.StageNotFinallyCertified && stc.StageCertifiedSeed)
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Conditioned seed can only be not-finally-certified or certified, not both; ";
            }
            if(stc.StageConditioned && !(stc.StageNotFinallyCertified || stc.StageCertifiedSeed))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If you select conditioned, you must also select not finally certified or certified; ";
            }
            if(!stc.StageInDirt && !stc.StageConditioned)
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "You must select either in-dirt or conditioned; ";
            }
            if((stc.StageFromField || stc.StageFromStorage) && (stc.StageNotFinallyCertified || stc.StageCertifiedSeed))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Can not have in-dirt and conditioned sub-stages checked at the same time; ";                
            }
            if(stc.StageOther && string.IsNullOrWhiteSpace(stc.StageOtherValue))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If you select other for seed status, you must provide a value; ";                
            }
            if(!stc.StageOther && !string.IsNullOrWhiteSpace(stc.StageOtherValue))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "You must select 'Other' if you provide a value for other; ";                
            }
            if(!stc.StageCertifiedSeed && (stc.StageTreatment || stc.StageBagging || stc.StageTagging || stc.StageBlending || stc.StageStorage || stc.StageOther))
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "If any seed status is checked, you must select Certified seed; ";
            }   
            if(stc.Pounds < 1)    
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Pounds Transferred must be a positive number; ";
            }
            if(string.IsNullOrWhiteSpace(stc.PurchaserName))    
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Purchaser Name must be supplied; ";
            }
            if(string.IsNullOrWhiteSpace(stc.PurchaserAddressLine1))    
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Purchaser Address Line 1 must be supplied; ";
            }
            if(string.IsNullOrWhiteSpace(stc.PurchaserCity))    
            {
                returnList.HasWarnings = true;
                returnList.Error = returnList.Error + "Purchaser City must be supplied; ";
            }

            return returnList;
        }
    }
}