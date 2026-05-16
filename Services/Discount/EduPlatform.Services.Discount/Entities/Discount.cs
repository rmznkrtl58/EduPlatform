namespace EduPlatform.Services.Discount.Entities
{
	//Veritabanıma table olarak adını veriyoruz.
	[Dapper.Contrib.Extensions.Table("discount")]
	public class Discount
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		//yüzdelik olarak verilen değer
		public int Rate { get; set; }
		public string Code { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
