using EduPlatform.Services.Order.Domain.Common;

namespace EduPlatform.Services.Order.Domain.OrderAggregate
{
	public class Address:BaseValueObject
	{
		//Set edilme konusunda programcının kontrolune aldım private yaparak.
		public string Province { get;private set; }
		public string District { get; private set; }
		public string Street { get; private set; }
		public string ZipKode { get; private set; }
		public string AddressDetail { get; private set; }
		public Address(string province, string district, string street, string zipKode, string addressDetail)
		{
			Province = province;
			District = district;
			Street = street;
			ZipKode = zipKode;
			AddressDetail = addressDetail;
		}
		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return new object[]
			{ Province, District, Street, ZipKode, AddressDetail };
		}
	}
}
