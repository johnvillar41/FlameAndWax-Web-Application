using System;
using FlameAndWax.Data.Models;
using FlameAndWax.Data.Repositories;
using FlameAndWax.Services.Repositories;
using FlameAndWax.Services.Repositories.Interfaces;
using FlameAndWax.Services.Services;
using FlameAndWax.Services.Services.BaseInterface.Interface;
using FlameAndWax.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlameAndWax
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
            services.AddControllersWithViews();
            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.IsEssential = true;
                  options.SlidingExpiration = true;
                  options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
              });

            //Connection String
            services.AddSingleton<IConfiguration>(Configuration);

            //Dependency Injection Repository
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ICustomerReviewRepository, CustomerReviewRepository>();
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IMessageRepository, MessageRepository>();
            services.AddSingleton<IProductGalleryRepository, ProductGalleryRepository>();
            services.AddSingleton<IPreviouslyOrderedProductsRepository, PreviouslyOrderedProductsRepository>();
            services.AddSingleton<IShippingAddressRepository, ShippingAddressRepository>();

            //Dependency Injection Services
            services.AddSingleton<IProductsService, ProductService>();
            services.AddSingleton(typeof(IAccountBaseService<CustomerModel>), typeof(CustomerAccountService));
            services.AddSingleton<IHomeService, HomeService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<IContactUsService, ContactUsService>();
            services.AddSingleton<IOrdersService, OrdersService>();
            services.AddSingleton<IUserProfileService, UserProfileService>();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
