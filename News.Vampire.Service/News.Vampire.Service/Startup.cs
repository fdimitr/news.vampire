using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Constants;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Managers;
using News.Vampire.Service.Managers.Interfaces;
using News.Vampire.Service.Services;
using News.Web.Api.BusinessLogic;

namespace News.Vampire.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IConfiguration>(Configuration);
            services.AddGrpc();
            services.AddGrpcReflection();

            DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseNpgsql(Configuration.GetValue<string>(ConfigKey.ConnectionString)).Options;

            //For regular using
            services.AddSingleton<DbContextOptions<DataContext>>(dbContextOptions);
            //For migration
            //services.AddDbContext<DataContext>(options => options.UseNpgsql("Server=localhost;UserName=postgres;Password=sqlrubin;Port=5432;Database=news_db;"));

            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.ClientId = Configuration.GetValue<string>(ConfigKey.GoogleAuthClientId);
            //        options.ClientSecret = Configuration.GetValue<string>(ConfigKey.GoogleAuthSecretId);
            //    });
            //services.AddAuthorization();

            services.AddScoped<IAuthLogic, AuthLogic>();
            services.AddScoped<IGroupLogic, GroupLogic>();
            services.AddScoped<IUserGroupLogic, UserGroupLogic>();
            services.AddScoped<ISourceLogic, SourceLogic>();
            services.AddScoped<INewsItemLogic, NewsItemLogic>();
            services.AddScoped<ISubscriptionLogic, SubscriptionLogic>();
            services.AddScoped<IDownloadManager, DownloadManager>();

            services.AddHostedService<DownloadService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GroupService>();
                endpoints.MapGrpcService<UserGroupService>();
                endpoints.MapGrpcService<NewsItemService>();

                if (env.IsDevelopment())
                {
                    endpoints.MapGrpcReflectionService();
                }

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
