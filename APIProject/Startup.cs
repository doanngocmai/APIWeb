using APIProject.Domain;
using APIProject.Middleware;
using APIProject.Repository;
using APIProject.Repository.Interfaces;
using APIProject.Service;
using APIProject.Service.Interfaces;
using APIProject.Service.Services;
using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SendGrid.Helpers.Mail;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace APIProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddAutoMapper(typeof(Startup));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
                //.AllowCredentials());
            });
            services.AddAutoMapper(typeof(APIProject.Service.MappingProfile));
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            //check token [Authorize]
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("App", new OpenApiInfo { Title = "App API", Version = "App" });
                options.SwaggerDoc("Web", new OpenApiInfo { Title = "Web API", Version = "Web" });

                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Description = "JWT containing userid claim",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                var security =
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "bearerAuth",
                                    Type = ReferenceType.SecurityScheme
                                },
                                UnresolvedReference = true
                            },
                            new List<string>()
                        }
                    };
                options.AddSecurityRequirement(security);
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
                options.EnableAnnotations();

            });
            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ConfigureCoreAndRepositoryService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("sia-firebase-sdk.json"),
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //sử dụng swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/App/swagger.json", "App API");
                c.SwaggerEndpoint("/swagger/Web/swagger.json", "Web API");
                c.RoutePrefix = string.Empty;
                c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
            });
            app.UseMiddleware<JWTMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=LoginWeb}/{id?}");
            });
            // Using service Get Ip From customer 
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });
        }
        private void ConfigureCoreAndRepositoryService(IServiceCollection services)
        {
            // basse
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IServices<>), typeof(BaseService<>));
            services.AddScoped<IHomeService, HomeService>();
            //services.AddScoped(typeof(IApplicationReadDbConnection), typeof(ApplicationReadDbConnection));
            //services.AddScoped(typeof(IApplicationWriteDbConnection), typeof(ApplicationWriteDbConnection));
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IConfigRepository, ConfigRepository>();
            services.AddScoped<IConfigService, ConfigSevice>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ISurveyQuestionRepository, SurveryQuestionRepository>(); 
            services.AddScoped<ISurveyService, SurveyService>();

            services.AddScoped<ISurveyAnswerRepository, SurveryAnswerRepository>();
            services.AddScoped<IGiftNewsRepository, GiftNewsRepository>();

            services.AddScoped<IStallRepository, StallRepository>();
            services.AddScoped<IStallService, StallService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IMemberPointHistoryRepository, MemberPointHistoryRepository>();
            services.AddScoped<IMemberPointHistoryService, MemberPointHistoryService>();
            services.AddScoped<IRelatedStallRepository, RelatedStallRepository>();
            services.AddScoped<IRelatedStallService, RelatedStallService>();
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IGiftRepository, GiftRepository>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IGiftCodeQRRepository, GiftCodeQRRepository>();
            services.AddScoped<IGiftCodeService, GiftCodeService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();

            //services.AddScoped<IServiceProvider, ServiceProvider>();//vừa thêm
            
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IWardRepository, WardRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IEventParticipantService, EventParticipantService>();
            services.AddScoped<IEventParticipantRepository, EventParticipantRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IQRCodeRepository, QRCodeRepository>();
            services.AddScoped<IQRCodeBillRepository, QRCodeBillRepository>();
            services.AddScoped<IEventChannelRepository, EventChannelRepository>();
            services.AddScoped<IGiftEventRepository, GiftEventRepository>();
            services.AddScoped<IGiftEventParticipantRepository, GiftEventParticipantRepository>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            services.AddScoped<IMapService, MapService>();
            services.AddScoped<ISurveySheetRepository, SurveySheetRepository>();
            services.AddScoped<ISendSmsService, SendSmsService>();
            services.AddScoped<IStatisticService, StatisticService>();

            // Add Mapter Singler 
            var mp = new MapperConfiguration((MapperContext) => MapperContext.AddProfile(new MappingProfile()));
            services.AddSingleton(mp.CreateMapper());

        }
    }
}
