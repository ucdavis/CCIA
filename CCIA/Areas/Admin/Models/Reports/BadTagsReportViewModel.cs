using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;
using Microsoft.Data.SqlClient;
using static Humanizer.On;

namespace CCIA.Models
{

	public class AdminBadTagsReportViewModel
	{

		public List<TagsReport> tagsReport { get; set; }

        [DataType(DataType.Date)] 
		public DateTime starDate { get; set; }
        [DataType(DataType.Date)] 
		public DateTime endDate { get; set; }





		public static async Task<AdminBadTagsReportViewModel> Create(CCIAContext _dbContext, AdminBadTagsReportViewModel vm)
		{
			var model = new AdminBadTagsReportViewModel();
			if (vm.starDate != DateTime.MinValue || vm.endDate != DateTime.MinValue)
			{
				var startDateParam = new SqlParameter("startDate", vm.starDate);
				var endDateParem = new SqlParameter("endDate", vm.endDate);
				model = new AdminBadTagsReportViewModel
				{
					starDate = vm.starDate,
					endDate = vm.endDate,
					tagsReport = await _dbContext.TagsReport.FromSqlRaw($"EXEC mvc_report_tags_with_bad_sids @startdate, @enddate", startDateParam, endDateParem).ToListAsync(),
				};
			}
			else
			{

				model = new AdminBadTagsReportViewModel
				{
					starDate = DateTime.Now.AddMonths(-1),
					endDate = DateTime.Now,
				};
			}

			return model;
		}


	}

}
