using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTRS.Core.Mapping;
using MTRS.DAL.DbContexts;
using MTRS.DAL.Interfaces;
using MTRS.DAL.Repositories;
using MTRS.Services;
using MTRS.Services.Interfaces;
using MTRS.Web.Auth;
using MTRS.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTRS.Web
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
            services.AddDbContext<MTRSDBContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MTRSDBContext).Assembly.FullName)));

            services.AddDbContext<LoggerDBContext>(options =>
           options.UseSqlServer(
               Configuration.GetConnectionString("MTRSLogsConnection"),
               b => b.MigrationsAssembly(typeof(LoggerDBContext).Assembly.FullName)));

            services.AddAuthentication(
                   CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   options =>
                   {
                       options.LoginPath = "/Account/Login";
                       options.LogoutPath = "/Account/Logout";
                   });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
           
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));


            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();
            services.AddScoped<IProjectUserRepository, ActivityUserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<ITimeSheetDetailsRepository, TimeSheetDetailsRepository>();
            services.AddScoped<ITimeSheetApprovalRepository, TimeSheetApprovalRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IBaseActivityRepository, BaseActivityRepository>();

            services.AddScoped<ILogRepository, LogRepository>();
            #endregion

            #region Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<ITimeSheetService, TimeSheetService>();
            services.AddScoped<ITimeSheetDetailsService, TimeSheetDetailsService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IBaseActivityService, BaseActivityService>();
            services.AddScoped<ITimeSheetApprovalService, TimeSheetApprovalService>();
            services.AddScoped<IPLDataService, PLDataService>();

            #endregion

            services.AddScoped<MTRSDBContext, MTRSDBContext>();
            services.AddScoped<LoggerDBContext, LoggerDBContext>();

            services.AddScoped<UserManager, UserManager>();
            services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            services.AddControllersWithViews();

            // authentication 
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
