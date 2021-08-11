using BlockService.Auth;
using BlockService.Entities;
using BlockService.Logger;
using BlockService.Repositories;
using BlockService.Repositories.FollowingMock;
using BlockService.Repositories.UserMock;
using BlockService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockService
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
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters(); //podrska za application/xml

            services.AddDbContext<ContextDB>(o => o.UseSqlServer(Configuration.GetConnectionString("Database")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IBlockingService, BlockingService>();//svaki put kada dodje req, napravi novu instancu ovoga
            services.AddScoped<IBlockingRepository, BlockingRepository>();
            services.AddScoped<IUserMockRepository, UserMockRepository>();
            services.AddScoped<IFollowingMockRepository, FollowingMockRepository>();
            services.AddSingleton(typeof(ILoggerRepository<>), typeof(LoggerRepository<>));

            services.AddScoped<IAuthService, AuthService>();
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("UserBlockingsApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo() //definise kako se kreira swagger dokument
                     {
                         Title = "User blocking API",
                         Version = "1",
                         Description = "API koji omogucava pregled blokiranja korisnika, nova blokiranja, izmenu i brisanje postojecih blokiranja.",
                         Contact = new Microsoft.OpenApi.Models.OpenApiContact
                         {
                             Name = "Sofija Djordjevic",
                             Email = "sofija.djordjevic98@gmail.com"
                         },
                         License = new Microsoft.OpenApi.Models.OpenApiLicense
                         {
                             Name = "FTN licenca"
                         }
                     });

                //var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml"; //refleksija
                //var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments); //spaja vise stringova

                //setupAction.IncludeXmlComments(xmlCommentsPath); //da bi swagger mogao citati xml komenatare

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("There has been error. Please try later!");
                    });
                });
            }

            app.UseSwagger();
            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/UserBlockingsApiSpecification/swagger.json", "User blocking API");
                setupAction.RoutePrefix = ""; //odmah mi otvori swagger dokumentaciju kada pokrenem servis u browseru
            });

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
