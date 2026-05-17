using EduPlatform.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

//IoC Registirations
builder.Services.AddCoreMvcRegistiration(builder.Configuration);

var app = builder.Build();

//MiddleWare configurations
app.UseConfigurePipelineExt();