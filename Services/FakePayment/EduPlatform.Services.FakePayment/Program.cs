using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//JwtBearer Configuration mikroservicimi koruma altýna alma.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
	opt.Authority = builder.Configuration["IdentityServerURL"];
	opt.Audience = "resource_fake_payment";
	opt.RequireHttpsMetadata = false;
});


//Jwt içerisindeki sub keyine ait kullanýcý Id valuemi maplame iþlemini düzeltmek için
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


//Default Controllers
var authorizePolicy = new
	AuthorizationPolicyBuilder()
	.RequireAuthenticatedUser().Build();//mutlaka authentice olmuþ kullanýcý gerekli.
builder.Services.AddControllers(opt =>
{
	//global authorize filter
	opt.Filters.Add(new AuthorizeFilter(authorizePolicy));
});



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
