using StackExchange.Redis;

namespace EduPlatform.Services.Basket.Services
{
	public class RedisService
	{
		public RedisService(string host, int port)
		{
			_host = host;
			_port = port;
		}
		private readonly string _host;
		private readonly int _port;
		//Ağır çalışan hemde bana bağlantımı sağlicak yapı
		private ConnectionMultiplexer _connectionMultiplexer;

		public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");
		public IDatabase GetDb(int dbNumber = 1) => _connectionMultiplexer.GetDatabase(dbNumber);
	
	}
}
