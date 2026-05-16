namespace EduPlatform.Services.Order.Domain.Common
{
	//marker yani işaretleyici gibi düşünebiliriz.
	//burdaki maksat şu ordere IAggregateRoot belirlediğimiz zaman
	//ordera bağlı bir entitiyim varsa ilişkili olandan yani orderItemdan
	//order entitiym ile bütünleşik davranacağız.
	public interface IAggregateRoot
	{
	}
}
