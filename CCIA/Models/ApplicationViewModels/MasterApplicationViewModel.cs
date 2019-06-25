namespace CCIA.Models
{
    // Created for cleaner reusability of partials among Applications
    // (Ex: Need to pass model properties around planting stocks partials, 
    //  so need to be aware of AppViewModel while also remaining application-agnostic).
    public class MasterApplicationViewModel : Applications
    {
        public virtual ApplicationViewModel AppViewModel { get; set; }
    }
}