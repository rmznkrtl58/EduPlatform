using EduPlatform.Services.Catalog.Extensions;

var builder = WebApplication.CreateBuilder(args);
//IoC Registirations
builder.Services.AddCatalogRegistirations(builder.Configuration);


//MiddleWares
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
