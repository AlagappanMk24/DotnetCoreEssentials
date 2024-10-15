using JwtSwaggerAuth.Integrations;
using JwtSwaggerAuth.Settings;
using JwtSwaggerAuth.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

// Add authorization services
builder.Services.AddAuthorization();

// Add controllers
builder.Services.AddControllers();

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
    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
    System.IO.File.WriteAllText(appSettingsFile, output);
}

// Register JWT settings as a singleton service
builder.Services.AddSingleton(jwtSettings);
builder.Services.RegisterServices();
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
