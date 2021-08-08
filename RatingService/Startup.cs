using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RatingService.Auth;
using RatingService.Entities;
using RatingService.Logger;
using RatingService.Repositories;
using RatingService.Repositories.BlockingMock;
using RatingService.Repositories.FollowingMock;
using RatingService.Repositories.PostMock;
using RatingService.Repositories.UserMock;
using RatingService.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RatingService
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

            services.AddScoped<IRatingService, RatingsService>();//svaki put kada dodje req, napravi novu instancu ovoga
            services.AddScoped<IRatingTypeService, RatingTypeService>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRatingTypeRepository, RatingTypeRepository>();
            services.AddScoped<IBlockingMockRepository, BlockingMockRepository>();
            services.AddScoped<IUserMockRepository, UserMockRepository>();
            services.AddScoped<IFollowingMockRepository, FollowingMockRepository>();
            services.AddSingleton<IPostMockRepository, PostMockRepository>();
            services.AddSingleton(typeof(ILoggerRepository<>), typeof(LoggerRepository<>));

            services.AddScoped<IAuthService, AuthService>();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("RatingsApiSpecification",
                     new Microsoft.OpenApi.Models.OpenApiInfo() //definise kako se kreira swagger dokument
                     {
                         Title = "Ratings to users' posts API",
                         Version = "1",
                         Description = "API koji omogucava pregled ocena na objavama korisnika, dodavanje novih ocena, izmenu, kao i brisanje postojecih ocena.",
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

                var xmlComments = $"{Assembly.GetExecutingAssembly().GetName().Name }.xml"; //refleksija
                var xmlCommentsPath = Path.Combine(AppContext.BaseDirectory, xmlComments); //spaja vise stringova

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

            app.UseSwagger();
            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint("/swagger/RatingsApiSpecification/swagger.json", "Ratings to users' posts API");
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
