namespace EduPlatform.Web.Options
{
	public class ClientSettingOptions
	{
		public const string Key= "ClientSettings";
		//appsettingsde doğru okumak için aynı olmalı isimlendirmeler
		public Client WebClient { get; set; }
		public Client WebClientForUser { get; set; }
	}
	public class Client
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
	}
}
