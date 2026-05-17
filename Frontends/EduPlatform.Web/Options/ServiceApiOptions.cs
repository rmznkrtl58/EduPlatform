namespace EduPlatform.Web.Options
{
	public class ServiceApiOptions
	{
		public const string Key = "ServiceApiSettings";
		public string IdentityBaseUri { get; set; }
		public string GatewayBaseUri { get; set; }
		public string PhotoStockUri { get; set; }
	}
}
