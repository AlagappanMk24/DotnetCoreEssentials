using JwtAuthDB.Data.Context;
using JwtAuthDB.Data.Repositories.Interface;
using JwtAuthDB.Data.Repositories;
using JwtAuthDB.Services.Interface;
using JwtAuthDB.Services;
using JwtAuthDB.Settings;
using JwtAuthDB.Utilities;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JwtContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("JWTConnection")));

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the token.",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Load JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Check if SecretKey is null or empty and generate a new one if necessary
if (string.IsNullOrEmpty(jwtSettings.SecretKey))
{
    var secretKey = SecretKeyGenerator.GenerateSecretKey();
    jwtSettings.SecretKey = secretKey;

    // Update appsettings.json with the new secret key
    var appSettingsFile = "appsettings.json";
    var json = System.IO.File.ReadAllText(appSettingsFile);
    dynamic jsonObj = JsonConvert.DeserializeObject(json);

    jsonObj["JwtSettings"]["SecretKey"] = secretKey;
    string output = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
    System.IO.File.WriteAllText(appSettingsFile, output);
}

// Register JWT settings as a singleton service
builder.Services.AddSingleton(jwtSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
