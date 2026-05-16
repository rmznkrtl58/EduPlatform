using EduPlatform.Services.Order.Application.Extensions;
using EduPlatform.Services.Order.Extensions;
using EduPlatform.Services.Order.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


//IoC
builder.Services.AddInfrustructure(builder.Configuration)
	.AddApplication(builder.Configuration).AddOrderApi(builder.Configuration);


//Middlewares
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
