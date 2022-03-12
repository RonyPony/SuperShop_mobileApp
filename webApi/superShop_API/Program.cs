using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using superShop_API.Database;
using superShop_API.Database.Entities.Auth;
using superShop_API.Database.Repositories.Constructor;
using superShop_API.Database.Services.Constructor;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SuperShop API - JWT Authentication with Swagger demo",
        Description = "Implementing JWT Authentication in SuperShop API",
        TermsOfService = new Uri("http://www.yansimora.com"),
        Contact = new OpenApiContact
        {
            Name = "Yansi Mora",
            Email = "yansimora@hotmail.com",
            Url = new Uri("http://www.yansimora.com")
        },
        License = new OpenApiLicense
        {
            Name = "Private Lincence",
            Url = new Uri("http://www.yansimora.com")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
    });
});

builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();
builder.Services.AddTransient<IRepositoryConstructor>(factory => new RepositoryConstructor(factory.GetService<DatabaseContext>()));
builder.Services.AddTransient<IServiceConstructor>(factory => new ServiceConstructor(factory.GetService<IRepositoryConstructor>()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(b => b.WithOrigins("http://localhost:4200/", "https://supershop-dashboard.web.app/")));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
