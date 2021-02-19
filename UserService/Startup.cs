using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Entities;

namespace UserService
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
            //Content negotiation in Accept header of users request
                setup.ReturnHttpNotAcceptable = true
            ).AddXmlDataContractSerializerFormatters();
            services.AddDbContext<UserDbContext>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IPersonalUserRepository, PersonalUserRepository>();
            services.AddScoped<ICorporationUserRepository, CorporationUserRepository>();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("ExamRegistrationOpenApiSpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "User Service API",
                        Version = "1",
                        Description = "API for creating, updating and fetcing users, roles and cities",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Natalija Gajić",
                            Email = "nat.gaj98@mail.com",
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense
                        {
                            Name = "FTN licence"
                        }
                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/ExamRegistrationOpenApiSpecification/swagger.json", "Student Exam Registration API");
                setupAction.RoutePrefix = ""; //No /swagger in url
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
