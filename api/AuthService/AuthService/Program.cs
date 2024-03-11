using AuthService.Data;
using AuthService.Models.Entities;
using AuthService.Profiles;
using AuthService.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using AuthService.Services.TokenProviders;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var jwtAuthOptions = builder.Configuration.GetSection("JwtSettings").Get<JwtAuthOptions>();

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserTwoFactorTokenProvider<ApplicationUser>, EmailConfirmationTokenProvider<ApplicationUser>>();
builder.Services.AddAutoMapper(typeof(ApplicationUserProfile));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("EmailConfirmation");

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtAuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtAuthOptions.Audience,
            ValidateLifetime = true,
            LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                notBefore <= DateTime.UtcNow && expires > DateTime.UtcNow,
            IssuerSigningKey = jwtAuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Service API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
var clientOrigin = builder.Configuration.GetValue<string>("Client");
builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", builder =>
    {
        builder.WithOrigins(clientOrigin)
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Development environment specific configurations
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Client");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
