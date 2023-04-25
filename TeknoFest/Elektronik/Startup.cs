using BuissnessLayer.Abstract;
using BuissnessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using ElektronikWebUI.EmailServices;
using ElektronikWebUI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;


namespace Elektronik
{
    public class Startup
    {
        
	
        public IConfiguration _configuration { get; set; }
        public Startup(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("server=LAPTOP-LM2N83TK;database=ElecktronikDb;integrated security=true;"));
            services.AddIdentity<Customer,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;

                options.Lockout.MaxFailedAccessAttempts= 5;
                options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers= true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan= TimeSpan.FromDays(1);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly= true,
                    Name=".Metin.Security.Cookie",
                    SameSite=SameSiteMode.Strict
                };
            });

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<ILikeService, LikeManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICategoryRepository,EfCoreCategoryRepository>();
            services.AddScoped<ICartRepository,EfCoreCartRepository>();
            services.AddScoped<ILikeRepository,EfCoreLikeRepository>();
            services.AddScoped<IOrderRepository,EfCoreOrderRepository>();

            services.AddScoped<IEmailSender,SmtpEmailSender>(i=> new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                 _configuration["EmailSender:UserName"],
                 _configuration["EmailSender:Password"]
                ));

			//services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
			//   new SmtpEmailSender(
			//	   _configuration["EmailSender:Host"],
			//	   _configuration.GetValue<int>("EmailSender:Port"),
			//	   _configuration.GetValue<bool>("EmailSender:EnableSSL"),
			//	   _configuration["EmailSender:UserName"],
			//	   _configuration["EmailSender:Password"])
			//   );

			services.AddControllersWithViews();
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

           
            app.UseEndpoints(endpoints =>
            {

               
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=page}/{id?}");
                
            });
        }
    }
}
