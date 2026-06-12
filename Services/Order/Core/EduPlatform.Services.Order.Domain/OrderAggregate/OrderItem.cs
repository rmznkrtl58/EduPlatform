using EduPlatform.Services.Order.Domain.Common;

namespace EduPlatform.Services.Order.Domain.OrderAggregate
{
	public class OrderItem:BaseEntity
	{
		//ürünüm kurs domaini üzerinden ilerldiği için courseId diyorum
		public string CourseId { get;private set; }
		public string CourseName { get; private set; }
		public string PictureUrl { get; private set; }
		public decimal Price { get; private set; }
		public OrderItem()
		{
			
		}
		public OrderItem(string courseId, string courseName, string pictureUrl, decimal price)
		{
			CourseId = courseId;
			CourseName = courseName;
			PictureUrl = pictureUrl;
			Price = price;
		}
		public void UpdateOrSave(string courseName,string pictureUrl,decimal price)
		{
			CourseName = courseName;
			PictureUrl = pictureUrl;
			Price = price;
		}
	}

}
