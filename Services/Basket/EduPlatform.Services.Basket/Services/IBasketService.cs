using EduPlatform.Services.Basket.Dtos;
using EduPlatform.Shared.Dtos;

namespace EduPlatform.Services.Basket.Services
{
	public interface IBasketService
	{
		Task<ResponseDto<BasketDto>> GetBasketByUser(string userId);
		Task<ResponseDto<ResponseMessageDto>> SaveOrUpdate(BasketDto p);
		Task<ResponseDto<bool>> Delete(string userId);
	}
}
