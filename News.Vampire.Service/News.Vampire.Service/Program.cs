using System.Text;
using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Constants;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.Managers.Interfaces;
using News.Vampire.Service.Managers;
using News.Vampire.Service.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using News.Vampire.Service.Models;
using News.Vampire.Service.Models.Dto;
using News.Vampire.Service.Models.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using News.Vampire.Service.Models.UserManagement;
using News.Vampire.Service.Services.Interfaces;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Models
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<GroupDto>("Groups").EntityType.Name = nameof(Group);
modelBuilder.EntitySet<Source>("Sources").EntityType.Name = nameof(Source);

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//For Entity Framework DbContext
DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>()
    .UseNpgsql(builder.Configuration.GetValue<string>(ConfigKey.ConnectionString)).Options;

builder.Services.AddSingleton(dbContextOptions);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>(ConfigKey.ConnectionString));
});
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>(ConfigKey.ConnectionString));
});

// Auto mapper
builder.Services.AddAutoMapper(typeof(ModelMappingProfile));

// Authentication
// Add Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Config Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ??
                                                                               throw new InvalidOperationException("Jwt secret doesn't specify")))
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();

// OData
builder.Services.AddControllers().AddOData(
    options => options
    .Select()
    .Filter()
    .OrderBy()
    .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthLogic, AuthLogic>();
builder.Services.AddScoped<IGroupLogic, GroupLogic>();
builder.Services.AddScoped<IUserGroupLogic, UserGroupLogic>();
builder.Services.AddScoped<ISourceLogic, SourceLogic>();
builder.Services.AddScoped<INewsItemLogic, NewsItemLogic>();
builder.Services.AddScoped<ISubscriptionLogic, SubscriptionLogic>();
builder.Services.AddScoped<IDownloadManager, DownloadManager>();
builder.Services.AddHostedService<DownloadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
