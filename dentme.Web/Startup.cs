using DAL;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using DAL.Repositories.Base.Repository;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using nevladinaOrg.Web.Helpers.Breadcrumb;
using nevladinaOrg.Web.Helpers.Culture;
using nevladinaOrg.Web.Helpers.GoogleMapsHelper;
using nevladinaOrg.Web.Helpers.ImageHelper;
using nevladinaOrg.Web.Helpers.Logger;
using nevladinaOrg.Web.Helpers.ModelBinders;
using nevladinaOrg.Web.Helpers.SelectListHelper;
using nevladinaOrg.Web.Helpers.UserFunctionalities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Reflection;

namespace nevladinaOrg.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                         .SetBasePath(_hostingEnvironment.ContentRootPath)
                         .AddJsonFile("appsettings.json")
                         .AddJsonFile($"appsettings.{_hostingEnvironment.EnvironmentName.ToLower()}.json", true)
                         .AddEnvironmentVariables();

            _configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();

            if (!_hostingEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
            }

            ConfigureDatabases(services);

            ConfigureDataUnitOfWork(services);

            ConfigureCustomServices(services);

            #region General

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddLocalization(options => options.ResourcesPath = Configuration.ResourcesPath);
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = Configuration.ResourcesPath)
            .AddDataAnnotationsLocalization(options =>
            {
                var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(Configuration.SharedResourceFileName, assemblyName.Name);
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Configuration.DefaultCulture, Configuration.DefaultCulture);
                options.SupportedCultures = options.SupportedUICultures = Configuration.SupportedSystemLanguages.Select(sl => sl.CultureInfo).ToList();
            });

            #endregion
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
                applicationBuilder.UseStatusCodePagesWithRedirects(Configuration.ErrorsPath)
                                  .UseHsts();
            }

            var localizationOptions = applicationBuilder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            applicationBuilder.UseRequestLocalization(localizationOptions.Value)
                              .UseHttpsRedirection()
                              .UseSession()
                              .UseStaticFiles();

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "areaRoute",
                        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });
        }



        private void ConfigureDatabases(IServiceCollection services)
        {
            var mainDatabaseConnectionString = _configuration.GetConnectionString(Configuration.MainDatabaseConnectionString);
            services.AddDbContextPool<NevladinaOrgContext>(options => options.UseSqlServer(mainDatabaseConnectionString));
        }

        private void ConfigureDataUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IDataUnitOfWork, DataUnitOfWork>();
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Localizer>();
            services.AddSingleton<GoogleMapsApiHelper>();

            //Helpers
            services.AddScoped<IUserFunctionalities, UserFunctionalities>();
            services.AddScoped<ISelectListHelper, SelectListHelper>();
            services.AddScoped<ILogger, Logger>();
            services.AddScoped<ICulture, Culture>();
            services.AddScoped<IBreadcrumb, Breadcrumb>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IActivityLogsRepository, ActivityLogsRepository>();

        }
    }
}
