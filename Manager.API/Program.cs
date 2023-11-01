global using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using Manager.API.Token;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// ToDo: --------------------------------------------------------------------------------------------------------
// ToDo: O error está aqui :(
// var secretKey = builder.Configuration["Jwt:Key"];
//
// builder.Services.AddAuthentication(x =>
//     {
//         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(x =>
//     {
//         x.RequireHttpsMetadata = false;
//         x.SaveToken = true;
//         x.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//     });
// ToDo: Fim do error :)
// ToDo: --------------------------------------------------------------------------------------------------------

AutoMapperDependenceInjection();

void AutoMapperDependenceInjection()
{
    var autoMapperConfig = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<User, UserDTO>().ReverseMap();
        cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
        cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
    });
    
    builder.Services.AddSingleton(autoMapperConfig.CreateMapper());
}


builder.Services.AddSingleton(d => builder.Configuration);

// ToDo: --------------------------------------------------------------------------------------------------------
// ToDo: Deu um error na connection string com o banco de dados, arrumei e deixei asssim: 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
builder.Services.AddDbContext<ManagerContext>(options =>
{
    options
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});
// ToDo: --------------------------------------------------------------------------------------------------------

builder.Services.AddMvc();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Manager API",
        Version = "v1",
        Description = "API construída na serie de vídeos no canal Lucas Eschechola.",
        Contact = new OpenApiContact
        {
            Name = "Lucas Eschechola",
            Email = "lucas.gabriel@eu.com",
            Url = new Uri("https://eschechola.com.br")
        },
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor utilize Bearer <TOKEN>",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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
