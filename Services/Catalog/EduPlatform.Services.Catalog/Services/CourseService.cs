using AutoMapper;
using EduPlatform.Services.Catalog.Dtos.CourseDtos;
using EduPlatform.Services.Catalog.Entities;
using EduPlatform.Services.Catalog.Settings;
using EduPlatform.Shared.Dtos;
using EduPlatform.Shared.Messages;
using MassTransit;
using MongoDB.Driver;
using System.Net;

namespace EduPlatform.Services.Catalog.Services
{
	public class CourseService:ICourseService
	{
		private readonly IMongoCollection<Course> _courseCollection;
		private readonly IMongoCollection<Category> _categoryCollection;
		private readonly IMapper _mapper;
		private readonly IPublishEndpoint _publishEndpoint;
		public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
		{
			_mapper = mapper;
			var client = new MongoClient(databaseSettings.ConnectionString);
			var db = client.GetDatabase(databaseSettings.DatabaseName);
			_courseCollection = db.GetCollection<Course>(databaseSettings.CourseCollectionName);
			_categoryCollection = db.GetCollection<Category>(databaseSettings.CategoryCollectionName);
			_publishEndpoint = publishEndpoint;
		}
		public async Task<ResponseDto<IEnumerable<GetCourseDto>>> GetListAllAsync()
		{
			var courses = await _courseCollection.Find(categories => true).ToListAsync();
			if (courses.Any())//eğer dbde herhangi bir kurs mevcutsa
			{
				courses.ForEach(async x =>
				{
					x.Category = await _categoryCollection.Find<Category>(y => y.Id == x.CategoryId).FirstAsync();
				});           	  
			}
			else
			{
				courses = new List<Course>();
			}
			var mapValues = _mapper.Map<IEnumerable<GetCourseDto>>(courses);
			return ResponseDto<IEnumerable<GetCourseDto>>.Success(mapValues,HttpStatusCode.OK.GetHashCode());
			
		}
		public async Task<ResponseDto<GetCourseDto>>GetByIdAsync(string id)
		{
			var value = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
			if (value is null) return ResponseDto<GetCourseDto>.Fail("İlgili Id'ye göre kurs bulunamamıştır.",HttpStatusCode.NotFound.GetHashCode());
			value.Category = await _categoryCollection.Find<Category>(x => x.Id == value.CategoryId).FirstAsync();
			var mapValues = _mapper.Map<GetCourseDto>(value);
			return ResponseDto<GetCourseDto>.Success(mapValues, HttpStatusCode.OK.GetHashCode());

		}
		public async Task<ResponseDto<IEnumerable<GetCourseDto>>>GetListByUserId(string userId)
		{
			var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
			if (courses.Any())
			{
				courses.ForEach(async theUserCourse =>
				{
					theUserCourse.Category = await _categoryCollection.Find<Category>
					(c => c.Id == theUserCourse.CategoryId).FirstAsync();
				});
			}
			else
			{
				courses = new List<Course>();
			}
			var mapValues = _mapper.Map<IEnumerable<GetCourseDto>>(courses);
			return ResponseDto<IEnumerable<GetCourseDto>>.Success(mapValues, HttpStatusCode.OK.GetHashCode());
		}
		public async Task<ResponseDto<CreateCouseDto>>CreateAsync(CreateCouseDto p)
		{
			//var nameExist = await _courseCollection.FindAsync<Course>(x => x.Name == p.Name);
			//if(nameExist is not null)
			//{
			//	return ResponseDto<CreateCouseDto>.Fail("İlgili kursa ait isim db'de mevcuttur.", HttpStatusCode.Conflict.GetHashCode());
			//}
			var mapValue = _mapper.Map<Course>(p);
			mapValue.CreatedDate = DateTime.Now;
			await _courseCollection.InsertOneAsync(mapValue);
			var responseMapping = _mapper.Map<CreateCouseDto>(mapValue);
			return ResponseDto<CreateCouseDto>.Success(responseMapping, HttpStatusCode.Created.GetHashCode());
		}
		public async Task<ResponseDto<NoContent>> UpdateAsync(UpdateCourseDto p)
		{
			var mapValue = _mapper.Map<Course>(p);
			mapValue.CreatedDate = DateTime.Now;
			var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == p.Id, mapValue);
			if (result is null) return ResponseDto<NoContent>.Fail("İlgili Id'ye ait course bulunamadı", HttpStatusCode.NotFound.GetHashCode());
			//RabbitMQ'ye kuyruğa bir event yolluyorum sebebi hem bu yollayacağım eventi
			//sepet servisim dinleyecek hemde sipariş servisim dinleyecek.
			//normalde fake paymentta bir command yolluyorduk ordada bir kuyruk ismi tanımlıyoduk
			//kuyruk ismine eventlerde gerek yok.exchange'E gideceği için exchange'e subcribe olan
			//farklı mikroservislerim(basket&order) kuyruk oluşturup mesajı alabilecek.
			await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent()
			{
				CourseId = mapValue.Id,
				UpdatedName = p.Name
			});
			return ResponseDto<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
		}
		public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
		{
			var result = await _courseCollection.DeleteOneAsync(x=>x.Id==id);
			if (result.DeletedCount > 0)
			{
				return ResponseDto<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
			}
			else
			{
				return ResponseDto<NoContent>.Fail("İlgili Id'ye ait kurs bulunamadı.",HttpStatusCode.NotFound.GetHashCode());
			}
		}
	}
}
