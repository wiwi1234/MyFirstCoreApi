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
using MyFirstCoreData.Models;
using MyFirstCoreData.Interface;
using Unity;
using Microsoft.EntityFrameworkCore.Migrations;
using MyFirstCoreData.Repository;
using MyFirstCoreService.Interface;
using MyFirstCoreService;
using MyFirstCoreApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.EntityFrameworkCore;

namespace MyFirstCoreApi
{
    public class Startup
    {
        public void ConfigureContainer(IUnityContainer container)
        {
            //Repository
            container.RegisterType<IRepository<Student>, Repository<Student>>();
            //Service
            container.RegisterType<IStudentService, StudentService>();
            container.RegisterSingleton<JwtHelper>();
        }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //configuration manager
            services.AddDbContext<WilliamHighSchoolContext>(option =>
                option.UseSqlServer(Configuration.GetConnectionString("localhost")));

            //register swagger generator
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "V1 Docs",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "William",
                        Email = "william.huang@soohoobook.com"
                    }
                });
                option.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "V2 Docs",
                    Version = "v2",
                    Contact = new OpenApiContact()
                    {
                        Name = "William",
                        Email = "william.huang@soohoobook.com"
                    }
                });
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                //Add security definitions
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });
            });

            //register JWT Service
            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
                options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // �z�L�o���ŧi�A�N�i�H�q "sub" ���Ȩó]�w�� User.Identity.Name
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    // �z�L�o���ŧi�A�N�i�H�q "roles" ���ȡA�åi�� [Authorize] �P�_����
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                    // �@��ڭ̳��|���� Issuer
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),

                    // �q�`���ӻݭn���� Audience
                    ValidateAudience = false,
                    //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

                    // �@��ڭ̳��|���� Token �����Ĵ���
                    ValidateLifetime = true,

                    // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                    ValidateIssuerSigningKey = false,

                    // "1234567890123456" ���ӱq IConfiguration ���o
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSettings:SignKey")))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Enable middleware to serve generated swagger as a JSON endpoint
            app.UseSwagger();

            //Enable middleware to serve swagger-ui(html , js , css , etc.)
            //specifying swagger Json endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                option.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); //������

            app.UseAuthorization();  //�A���v

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
