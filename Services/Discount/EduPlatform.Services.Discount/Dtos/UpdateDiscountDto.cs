namespace EduPlatform.Services.Discount.Dtos
{
	public class UpdateDiscountDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		//yüzdelik olarak verilen değer
		public int Rate { get; set; }
		public string Code { get; set; }
	}
}
