using EduPlatform.Services.Order.Application.Extensions;
using EduPlatform.Services.Order.Extensions;
using EduPlatform.Services.Order.Infrastructure.Data;
using EduPlatform.Services.Order.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//IoC
builder.Services.AddInfrustructure(builder.Configuration)
	.AddApplication(builder.Configuration).AddOrderApi(builder.Configuration);


//Middlewares
var app = builder.Build();

//Docker ayağa kalktığı anda 500 hatası alıyoruz core mvc tarafında
//geçmiş siparişler gözükmüyor.o da docker içerisindeki volume bağlı
//yeni bir db oluşturduğundan dolayı direk 
//Uygulama ayağa kalktığında migrate etsin diyoruz.
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var orderDbContext = serviceProvider.GetRequiredService<AppDbContext>();
    orderDbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
