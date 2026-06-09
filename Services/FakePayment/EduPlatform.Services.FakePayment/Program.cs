using EduPlatform.Services.FakePayment.Extensions;

var builder = WebApplication.CreateBuilder(args);

//IoC Registirations
builder.Services.AddFakePayment(builder.Configuration);

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
