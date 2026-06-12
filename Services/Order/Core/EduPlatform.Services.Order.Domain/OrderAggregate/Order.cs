using EduPlatform.Services.Order.Domain.Common;
namespace EduPlatform.Services.Order.Domain.OrderAggregate
{
	public class Order:BaseEntity,IAggregateRoot
	{
		public DateTime CreatedDate { get;private set; }
		public Address Address { get; private set; }
		//Satın alan kişinin Id'si
		public string BuyerId { get; private set; }
        //EnCapsulation yapısını uyguladık.
		private readonly List<OrderItem> _orderItems;
		public IReadOnlyCollection<OrderItem>OrderItems=> _orderItems;
		public Order()
		{
		}
		public Order(Address address,string buyerId)
		{
			_orderItems = new List<OrderItem>();
			CreatedDate = DateTime.Now;
			Address = address;
			BuyerId= buyerId;
		}
		public void AddOrderItem(string courseId,string courseName,decimal price,string pictureUrl)
		{
			var existCourse = _orderItems.Any(c => c.CourseId == courseId);
			//bir şey yollanacak 
			if (!existCourse)
			{
				var newOrderItem = new OrderItem(courseId, courseName, pictureUrl, price);
				_orderItems.Add(newOrderItem);
			}
		}
		public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
	}
}
