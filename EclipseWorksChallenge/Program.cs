using EclipseWorksChallenge.MyData;
using EclipseWorksChallenge.MyData.MyEntities;
using EclipseWorksChallenge.MyData.MyRepositories;
using EclipseWorksChallenge.MySecurity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>();

//builder.Services
//.AddControllers()
//.ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressMapClientErrors = true;
//    options.SuppressModelStateInvalidFilter = true;
//});

builder.Services.AddSingleton<IMyJwtSigningManager, MyJwtSigningManager>();

builder.Services.AddScoped<IGenericRepository<Projeto>, GenericRepository<Projeto>>();
builder.Services.AddScoped<IGenericRepository<Tarefa>, GenericRepository<Tarefa>>();
builder.Services.AddScoped<IGenericRepository<Historico>, GenericRepository<Historico>>();
builder.Services.AddScoped<IGenericRepository<Comentario>, GenericRepository<Comentario>>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IMyJwtSigningManager>((options, signingManager) =>
    {
        options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidIssuer = "Me",
            ValidAudience = "Me",
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = signingManager.FetchCurrentEcdsaKey()
        };
    });

//var tokenKey = "MyUltraUnbelievableSecretKey@#$10";
//var key = Encoding.ASCII.GetBytes(tokenKey);
//.AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("MyHubPolicy2", policy =>
//    {
//        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
//        policy.RequireAuthenticatedUser();

//        //policy.RequireAuthenticatedUser();
//        policy.RequireRole("Banta");
//        //policy.RequireRole("Panta");
//    });

//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Description = "Por favor, forneça um token válido.",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
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

//app.UseHttpsRedirection();
//app.UseRouting();
//app.UseCors();
//app.UseAuthentication();
//app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
