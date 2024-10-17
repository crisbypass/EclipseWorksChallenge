using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<MyMvc.Identity_Data.ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

//builder.Services
//.AddControllers()
//.ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressMapClientErrors = true;
//    options.SuppressModelStateInvalidFilter = true;
//});

var tokenKey = "MyUltraUnbelievableSecretKey@#$10";
var key = Encoding.ASCII.GetBytes(tokenKey);

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
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
//    x.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            var accessToken = context.Request.Query["access_token"];
//            // If the request is for our hub(to SignalR, in fact)...
//            var path = context.HttpContext.Request.Path;
//            if (!string.IsNullOrEmpty(accessToken) &&
//                path.StartsWithSegments("/MySignalR/MyHub"))
//            {
//                // Read the token out of the query string
//                context.Token = accessToken;
//            }
//            return Task.CompletedTask;
//        }
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

//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
//        new OpenApiSecurityScheme
//        {
//            Description = "Please, provide token value.",
//            In = ParameterLocation.Header,
//            Name = "Authorization",
//            Type = SecuritySchemeType.Http,
//            BearerFormat = "JWT",
//            Scheme = "bearer"
//        });

//    options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
