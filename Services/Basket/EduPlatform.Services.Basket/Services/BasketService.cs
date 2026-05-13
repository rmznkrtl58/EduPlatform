using EduPlatform.Services.Basket.Dtos;
using EduPlatform.Shared.Dtos;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
namespace EduPlatform.Services.Basket.Services
{
	public class BasketService : IBasketService
	{
		private readonly RedisService _redisService;
		private readonly IDatabase _db;
		public BasketService(RedisService redisService)
		{
			_redisService = redisService;
			_db = _redisService.GetDb();
		}
		public async Task<ResponseDto<bool>> Delete(string userId)
		{
			var status = await _db.KeyDeleteAsync(userId);
			return status ?
				ResponseDto<bool>.Success(HttpStatusCode.NoContent.GetHashCode())
				:
				ResponseDto<bool>.Fail("İlgili kullanıcı bulunamadı!", HttpStatusCode.NotFound.GetHashCode());
		}

		public async Task<ResponseDto<BasketDto>> GetBasketByUser(string userId)
		{
			var existBasket = await _db.StringGetAsync(userId);
			if (string.IsNullOrEmpty(existBasket)) return ResponseDto<BasketDto>.Fail("İlgili Kullanıcıya ait Sepet Bulunamamıştır.",HttpStatusCode.BadRequest.GetHashCode());
			var basket = JsonSerializer.Deserialize<BasketDto>(existBasket!);
			return ResponseDto<BasketDto>.Success(basket!, HttpStatusCode.OK.GetHashCode());
		}

		public async Task<ResponseDto<ResponseMessageDto>> SaveOrUpdate(BasketDto p)
		{
			var status = await _db.StringSetAsync(p.UserId, JsonSerializer.Serialize(p));

			return status ?
				ResponseDto<ResponseMessageDto>.Success(new ResponseMessageDto() { Message = "Sepet başarıyla kaydedildi." }, HttpStatusCode.Created.GetHashCode())
				:
				ResponseDto<ResponseMessageDto>.Fail("Sepeti kaydederken veya güncellerken sorun oluştu!", HttpStatusCode.InternalServerError.GetHashCode());

		}
	}
}
