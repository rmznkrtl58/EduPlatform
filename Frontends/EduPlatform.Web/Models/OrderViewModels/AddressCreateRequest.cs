namespace EduPlatform.Web.Models.OrderViewModels
{
	public class AddressCreateRequest
	{
		public string Province { get; set; }
		public string District { get; set; }
		public string Street { get; set; }
		public string ZipKode { get; set; }
		public string AddressDetail { get; set; }
	}
}
