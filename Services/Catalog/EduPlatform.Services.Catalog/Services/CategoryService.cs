using AutoMapper;
using EduPlatform.Services.Catalog.Dtos.CategoryDtos;
using EduPlatform.Services.Catalog.Entities;
using EduPlatform.Services.Catalog.Settings;
using EduPlatform.Shared.Dtos;
using MongoDB.Driver;
using System.Net;

namespace EduPlatform.Services.Catalog.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IMongoCollection<Category> _categoryCollection;
		private readonly IMapper _mapper;
		public CategoryService(IMapper mapper,IDatabaseSettings databaseSettings)
		{
			_mapper = mapper;
			var client = new MongoClient(databaseSettings.ConnectionString);
			var db = client.GetDatabase(databaseSettings.DatabaseName);
			_categoryCollection = db.GetCollection<Category>(databaseSettings.CategoryCollectionName);
		}
		public async Task<ResponseDto<IEnumerable<GetCategoryDto>>> GetListAllAsync()
		{
			var values = await _categoryCollection.Find(category => true).ToListAsync();
			var mapValues = _mapper.Map<IEnumerable<GetCategoryDto>>(values);
			return ResponseDto<IEnumerable<GetCategoryDto>>.Success(mapValues,(int)HttpStatusCode.OK);
		}
		public async Task<ResponseDto<GetCategoryDto>> GetByIdAsync(string id)
		{
			var value = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();
			if (value is null) return ResponseDto<GetCategoryDto>.Fail("İlgili Id'ye ait kategori bulunamadı!",HttpStatusCode.NotFound.GetHashCode());
			var mapValue = _mapper.Map<GetCategoryDto>(value);
			return ResponseDto<GetCategoryDto>.Success(mapValue, HttpStatusCode.OK.GetHashCode());
		}
		public async Task<ResponseDto<CreateCategoryDto>>CreateAsync(CreateCategoryDto p)
		{
			var mapValue = _mapper.Map<Category>(p);
		    await _categoryCollection.InsertOneAsync(mapValue);
			return ResponseDto<CreateCategoryDto>.Success(p, HttpStatusCode.Created.GetHashCode());
		}
	}
}
