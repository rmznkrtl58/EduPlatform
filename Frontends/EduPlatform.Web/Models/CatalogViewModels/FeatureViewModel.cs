using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Web.Models.CatalogViewModels
{
	public class FeatureViewModel
	{
		[Display(Name ="Kurs Süresi")]
		public int Duration { get; set; }
	}
}
