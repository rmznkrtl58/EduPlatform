using Dapper;
using EduPlatform.Services.Discount.Dtos;
using EduPlatform.Services.Discount.Settings;
using EduPlatform.Shared.Dtos;
using Mapster;
using Npgsql;
using System.Data;
using System.Net;

namespace EduPlatform.Services.Discount.Services
{
	public class DiscountService : IDiscountService
	{
		private readonly IDbConnection _dbConnection;
		private readonly PostgreSqlSettings _postgreSqlSettings;
		public DiscountService(PostgreSqlSettings postgreSqlSettings)
		{
			_postgreSqlSettings = postgreSqlSettings;
			_dbConnection = new NpgsqlConnection(_postgreSqlSettings.PostgreSql);
		}

		
		public async Task<ResponseDto<ResponseMessageDto>> Create(CreateDiscountDto p)
		{
			var mapValue = p.Adapt<Entities.Discount>();
			mapValue.CreatedDate= DateTime.Now;
			//result değişkenim 1'se başarılı değilse başarısız
			var result=await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", mapValue);
			return result > 0 ? ResponseDto<ResponseMessageDto>.Success(new ResponseMessageDto { Message = "İndirim Kuponu oluşturuldu" }, HttpStatusCode.Created.GetHashCode())
							  : ResponseDto<ResponseMessageDto>.Fail("İndirim kuponu oluşurken hata oluştu", HttpStatusCode.InternalServerError.GetHashCode());
		}

		public async Task<ResponseDto<ResponseMessageDto>> Delete(int id)
		{
			var result = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new { Id = id });
			return result > 0
				? ResponseDto<ResponseMessageDto>.Success(new ResponseMessageDto { Message="İndirim kuponu silindi"},HttpStatusCode.NoContent.GetHashCode())
				: ResponseDto<ResponseMessageDto>.Fail("indirim kuponu bulanamadı!", HttpStatusCode.NotFound.GetHashCode());
		}

		public async Task<ResponseDto<GetDiscountDto>> GetById(int id)
		{
			var value = (await _dbConnection.QueryAsync<Entities.Discount>("select * from discount where id=@Id", new { Id=id })).SingleOrDefault();
			if (value is null) return ResponseDto<GetDiscountDto>.Fail("İlgili id'ye ait indirim kuponu bulunamadı!", HttpStatusCode.NotFound.GetHashCode());
		    
			var mapValue=value.Adapt<GetDiscountDto>();
			return ResponseDto<GetDiscountDto>.Success(mapValue, HttpStatusCode.OK.GetHashCode());
		}

		public async Task<ResponseDto<GetDiscountDto>> GetDiscountByCodeAndUserId(string coupenCode, string userId)
		{
			var hasDiscount = (await _dbConnection.QueryAsync<Entities.Discount>("select * from discount where userid=@UserId and code=@Code", new { UserId = userId, Code = coupenCode })).FirstOrDefault();
			if (hasDiscount is null) return ResponseDto<GetDiscountDto>.Fail("İlgili kullanıcıya ait kupon kodu bulunamadı", HttpStatusCode.NotFound.GetHashCode());
		    var mapValue=hasDiscount.Adapt<GetDiscountDto>();
			return ResponseDto<GetDiscountDto>.Success(mapValue, HttpStatusCode.OK.GetHashCode());
		}

		public async Task<ResponseDto<IEnumerable<GetDiscountDto>>> GetListAll()
		{
			var values = await _dbConnection.QueryAsync<Entities.Discount>("select * from discount");
			var mapValues = values.Adapt<IEnumerable<GetDiscountDto>>();
			return ResponseDto<IEnumerable<GetDiscountDto>>.Success(mapValues, HttpStatusCode.OK.GetHashCode());
		}

		public async Task<ResponseDto<ResponseMessageDto>> Update(UpdateDiscountDto p)
		{
			var mapValue=p.Adapt<Entities.Discount>();
			mapValue.CreatedDate = DateTime.Now;
			var result=await _dbConnection.ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id", new { Id = mapValue.Id, UserId = mapValue.UserId, Code = mapValue.Code, Rate = mapValue.Rate });
			return result > 0 ? ResponseDto<ResponseMessageDto>.Success(new ResponseMessageDto { Message = "İndirim Kuponu güncellendi!" }, HttpStatusCode.NoContent.GetHashCode())
							  : ResponseDto<ResponseMessageDto>.Fail("İndirim kuponu güncellenirken hata oluştu", HttpStatusCode.InternalServerError.GetHashCode());
		}
	}
}
