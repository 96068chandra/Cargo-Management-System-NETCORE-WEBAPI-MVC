using CargoManagementAPi.IRepository;
using CargoManagementAPi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace CargoManagementAPi
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
    //        services.AddIdentity<Customer, >(options =>
    //        {
    //            options.Password.RequireDigit = false;
    //            options.Password.RequireNonAlphanumeric = false;
    //            options.Password.RequireLowercase = false;
    //            options.Password.RequireUppercase = false;
    //            options.Password.RequiredLength = 6;
    //        }).AddEntityFrameworkStores<ApplicationDbContext>()
    //.AddDefaultTokenProviders();
    //        services.AddControllersWithViews();


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JWT:issuer"],
                ValidAudience = Configuration["JWT"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))

            });

            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("CargoConnection")));

            services.AddScoped<IRepository<Cargo>, CargoRepository>();
            services.AddScoped<IRepository2<Customer>, CustomerRepository>();
            services.AddScoped<IRepository3<Admin>, AdminRepository>();
            services.AddScoped<IRepositoryEmployee<Employee>, EmployeeRepository>();
            services.AddScoped<IRepositoryCODR<CargoOrderDetails>, CargoOrderDetailsRepository>();
            services.AddScoped<IRepository5<City>, CityRepository>();
            services.AddScoped<IRepository4<CargoType>, CargoTypeRepository>();
            services.AddScoped<IRepositoryCargoStatus<CargoStatus>, CargoStatusRepository>();



            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CargoManagementAPi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CargoManagementAPi v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
