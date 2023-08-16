﻿using Identity.Context;
using Identity.Model;
using Identity.Repository;
using Identity.Repository.Business;
using Identity.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Payroll.Core.Context;

namespace WebAPI.Utils
{
    public static class ServiceExtension
    {
        public static void ConfigureService(this IServiceCollection services)
        {
            // For Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IAuthenticateBL, AuthenticateBL>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<Payroll.Core.Repository.IRepositoryWrapper, Payroll.Core.Repository.RepositoryWrapper>();
            services.AddDbContext<PayrollContext>();
            //services.AddSingleton<ILoggerManager, LoggerManager>();

            //CONFIGURE CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
    }
}
