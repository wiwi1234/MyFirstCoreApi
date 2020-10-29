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
                // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                    // 一般我們都會驗證 Issuer
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("JwtSettings:Issuer"),

                    // 通常不太需要驗證 Audience
                    ValidateAudience = false,
                    //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                    // 一般我們都會驗證 Token 的有效期間
                    ValidateLifetime = true,

                    // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                    ValidateIssuerSigningKey = false,

                    // "1234567890123456" 應該從 IConfiguration 取得
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

            app.UseAuthentication(); //先驗證

            app.UseAuthorization();  //再授權

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
