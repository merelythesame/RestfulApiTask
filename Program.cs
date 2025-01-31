using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetSection("JWTSetting")["SecretKey"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = false,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWTSetting:Issuer"], 
		ValidAudience = builder.Configuration["JWTSetting:Audience"], 
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
	await next();

	if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
	{
		context.Response.ContentType = "application/json";
		await context.Response.WriteAsync("{\"error\": \"Access denied: You do not have the required permissions.\"}");
	}

	if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
	{
		context.Response.ContentType = "application/json";
		await context.Response.WriteAsync("{\"error\": \"Unauthorized: You are not logged in.\"}");
	}
});


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
