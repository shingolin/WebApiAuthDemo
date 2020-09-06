using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;
using WebApiAuthDemo.Controllers;
//using DocumentFormat.OpenXml.Bibliography;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Drawing.Diagrams;
//using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;

namespace WebApiAuthDemo
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
            services.AddControllers();
            services.AddScoped<UserValidateService>();
            services.AddScoped<UserInfoService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                            ValidIssuer = "JwtAuthDemo", // "JwtAuthDemo" ���ӱq IConfiguration ���o

                            // �Y�O��@���A���q�`���ӻݭn���� Audience
                            ValidateAudience = false,
                            //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

                            // �@��ڭ̳��|���� Token �����Ĵ���
                            ValidateLifetime = true,

                            // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                            ValidateIssuerSigningKey = false,

                            // "1234567890123456" ���ӱq IConfiguration ���o
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456"))
                        };
                    });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Test",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                                       {
                                         new OpenApiSecurityScheme
                                         {
                                           Reference = new OpenApiReference
                                           {
                                             Type = ReferenceType.SecurityScheme,
                                             Id = "Bearer"
                                           }
                                          },
                                          new string[] { }
                                        }
                                      });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "WebApiAuthDemo.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
