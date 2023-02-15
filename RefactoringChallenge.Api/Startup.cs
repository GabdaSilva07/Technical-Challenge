using System.IO;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RefactoringChallenge.Data.CQRS;
using RefactoringChallenge.Data.Factories;
using RefactoringChallenge.Entities;

namespace RefactoringChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationRoot = GetConfigurationRoot();

            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(configurationRoot.GetConnectionString("DefaultConnection")));
            
            
            services.AddSingleton(TypeAdapterConfig.GlobalSettings);
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen();

            var orderCommandQueryFactory =
                new OrderCommandQueryFactory(configurationRoot.GetConnectionString("DefaultConnection"));
            services.AddScoped<IQueryFactory<Order>>(provider => orderCommandQueryFactory);
            services.AddScoped<ICommandFactory<Order, Order>>(provider => orderCommandQueryFactory);

            var orderDetailCommandQueryFactory =
                new OrderDetailCommandQueryFactory(configurationRoot.GetConnectionString("DefaultConnection"));
            services.AddScoped<IQueryFactory<OrderDetail>>(provider => orderDetailCommandQueryFactory);
            services.AddScoped<ICommandFactory<OrderDetail, OrderDetail>>(provider => orderDetailCommandQueryFactory);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddEnvironmentVariables().Build();
        }
    }
}
