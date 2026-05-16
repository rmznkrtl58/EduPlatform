using EduPlatform.Services.Discount.Dtos;
using EduPlatform.Shared.Dtos;

namespace EduPlatform.Services.Discount.Services
{
	public interface IDiscountService
	{
		Task<ResponseDto<IEnumerable<GetDiscountDto>>> GetListAll();
		Task<ResponseDto<GetDiscountDto>> GetById(int id);
		Task<ResponseDto<ResponseMessageDto>> Create(CreateDiscountDto p);
		Task<ResponseDto<ResponseMessageDto>> Update(UpdateDiscountDto p);
		Task<ResponseDto<ResponseMessageDto>> Delete(int id);
		//olayım şu güvenlik açısından kontrol etmek için ilgili kullanıcıma
		//ait gerçekten bir indirim kodu tanımlanmışmı onun için userId ve 
		//ilgili indirim kodunu alıp dbde kontrol ettireceğim.
		Task<ResponseDto<GetDiscountDto>> GetDiscountByCodeAndUserId(string coupenCode, string userId);
	}
}
