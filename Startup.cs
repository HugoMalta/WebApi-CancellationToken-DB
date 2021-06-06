using CancellationTokenApi.ActionFilterExecuting;
using CancellationTonkenCommand.Handler;
using CancellationTonkenCommand.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace CancellationTokenApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDbConnection>(db =>
            {
                SqlConnection connection = new SqlConnection(Configuration.GetValue<string>("ConnectionString"));
                return connection;
            });
            services.AddMediatR(typeof(CategoryCommandHandler));
            _ = services.AddTransient<IRequestHandler<CategoryCommandRequest, CategoryCommandResponse>>(x => new CategoryCommandHandler(
                x.GetRequiredService<ICategoryRepository>()
                ));

            services.AddSingleton<ICategoryRepository, CategoryRepository>(x => new CategoryRepository(
                Configuration.GetValue<string>("ConnectionString"),
                x.GetRequiredService<IDbConnection>()
                
                ));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CancellationTokenApi", Version = "v1" });
            });
            services.AddMvc(options =>
            {
                options.Filters.Add<OperationCancelledExceptionFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CancellationTokenApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
