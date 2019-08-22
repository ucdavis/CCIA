using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public enum GermplasmSource
    {
        [Display(Name="Select germplasm type...")]
        SelectGermplasmType,
        [Display(Name="Source Identified Germplasm")]
        SourceIdentifiedGermplasm,
        [Display(Name="Selected Germplasm")]
        SelectedGermplasm,
        [Display(Name="Tested Germplasm")]
        TestedGermplasm
    }
}