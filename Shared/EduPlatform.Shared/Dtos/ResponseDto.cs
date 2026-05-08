using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EduPlatform.Shared.Dtos
{
	public class ResponseDto<T>
	{
		//private set,sadece buradan set ediliyor dışardan kimse set edemez.
		public T Data { get;private set; }
		[JsonIgnore]
		public int StatusCode { get; private set; }
		[JsonIgnore]
		public bool IsSuccessFul { get; private set; }
		public List<string> Errors { get; set; }
		
		//Static Factory Metods
		//Datalı başarılı
		public static ResponseDto<T> Success(T data,int statusCode)
		{
			return new ResponseDto<T>()
			{
				Data = data,
				StatusCode = statusCode,
				IsSuccessFul = true
			};
		}
		//Datasız başarılı
		public static ResponseDto<T> Success(int statusCode)
		{
			return new ResponseDto<T>()
			{
				Data = default(T),
				StatusCode = statusCode,
				IsSuccessFul = true
			};
		}
		//birden fazla hata başarısız
		public static ResponseDto<T> Fail(List<string>errors,int statusCode)
		{
			return new ResponseDto<T>()
			{
				Errors = errors,
				StatusCode = statusCode,
				IsSuccessFul = false
			};
		}
		//bir hata başarısız
		public static ResponseDto<T> Fail(string error,int statusCode)
		{
			return new ResponseDto<T>()
			{
				Errors = new List<string> { error },
				IsSuccessFul = false,
				StatusCode = statusCode
			};
		}
	}
}
