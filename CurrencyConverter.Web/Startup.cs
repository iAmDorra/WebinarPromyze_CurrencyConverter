using CurrencyConverter.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            InitializeDatabase();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase()
        {
            MigrateDatabase();
            CleanRatesTable();
            InsertNewRate("EUR", "USD", 1.14m);
        }

        private void MigrateDatabase()
        {
            using (var dbContext = new CurrencyConverterContext())
            {
                dbContext.Database.Migrate();
            }
        }

        private void InsertNewRate(string sourceCurrency, string targetCurrency, decimal usdRateValue)
        {
            using (var dbContext = new CurrencyConverterContext())
            {
                var usdRate = new RateValue
                {
                    RateValueId = 1,
                    Currency = sourceCurrency,
                    TargetCurrency = targetCurrency,
                    Value = usdRateValue
                };
                dbContext.Rates.Add(usdRate);
                dbContext.SaveChanges();
            }
        }

        private static void CleanRatesTable()
        {
            using (var dbContext = new CurrencyConverterContext())
            {
                var deleteQuery = $"delete from {nameof(dbContext.Rates)}";
                dbContext.Database.ExecuteSqlCommand(deleteQuery);
            }
        }
    }
}
