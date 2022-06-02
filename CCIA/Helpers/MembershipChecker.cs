using CCIA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Helpers
{
    public class MembershipChecker
    {
        public bool CurrentMember { get; set; }

        

        public static async Task<MembershipChecker> Check(CCIAContext _dbContext, int orgId)
        {
            var checker = new MembershipChecker();
            var lastMembershipYear = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.MemberYear.Value).FirstOrDefaultAsync();
            if(lastMembershipYear == 0 || lastMembershipYear < CertYearFinder.CertYear)
            {
                checker.CurrentMember = false;
            } else {
                checker.CurrentMember = true;
            }                     
            return checker;
        }
    }
}