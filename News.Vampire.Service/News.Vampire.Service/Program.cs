using Microsoft.EntityFrameworkCore;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Constants;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.Managers.Interfaces;
using News.Vampire.Service.Managers;
using News.Vampire.Service.Services;
using News.Web.Api.BusinessLogic;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using News.Vampire.Service.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// Models
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Group>("Groups");

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//For migration
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetValue<string>(ConfigKey.ConnectionString));
});


// Add services to the container.

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

//app.UseAuthorization();

app.MapControllers();

app.Run();
