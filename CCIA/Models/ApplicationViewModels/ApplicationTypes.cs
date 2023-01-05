using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public enum AppTypes
    {
        [Display(Name="SD")]
        Seed = 1,
        [Display(Name="PO")]
        Potato,
        [Display(Name="GQ")]
        GrainQA,
        [Display(Name="PV")]
        PrevarietyGermplasm,
        [Display(Name="RQ")]
        RiceQA,
        [Display(Name="TG")]
        Turfgrass,
        [Display(Name="HP")]
        HempProgarm
    }

    public enum CropIdNames
    {
        Alfalfa = 1,
        Rice = 63,
        Sunflower = 74,
        Potato = 137,
        Hemp = 316
    }
}