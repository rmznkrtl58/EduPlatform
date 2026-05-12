using EduPlatform.Services.PhotoStock.Extensions;
var builder = WebApplication.CreateBuilder(args);


//IoC Registirations
builder.Services.AddPhotoStockRegistirations(builder.Configuration);


//Middlewares
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//Eklediðimiz kaydettiðimiz dosyalarý dýþarýya açma örneðin "images"
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
