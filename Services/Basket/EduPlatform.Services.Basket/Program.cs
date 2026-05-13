using EduPlatform.Services.Basket.Extensions;
var builder = WebApplication.CreateBuilder(args);


//IoC
builder.Services.AddBasketRegistirations(builder.Configuration);


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
