using EduPlatform.Gateway.Extensions;
var builder = WebApplication.CreateBuilder(args);


//IoC Registirations
builder.Services.AddOcelotRegistiration(builder.Configuration,builder);


//Middlewares
var app = builder.Build();


await app.UseConfigurePipelineExt();