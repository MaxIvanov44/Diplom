using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAccounting.Logic.Services;
using StudentAccounting.Logic.Services.Implementations;
using StudentAccounting.Data.Repositories;
using StudentAccounting.Data.Repositories.Implementations;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using StudentAccounting.Data;
using System.IO;
using Microsoft.Extensions.FileProviders;
using StudentAccounting.Logic.Services.ReportGeneration;
using StudentAccounting.Data.EntityModels;
using Microsoft.AspNetCore.Identity;
using StudentAccounting.Logic.Services.EmailSender;
using MassTransit;
using StudentAccounting.Logic.EventBus;
using Microsoft.Extensions.Logging;
using GreenPipes;
using Serilog;

namespace StudentAccounting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<EmailConsumer>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumer<EmailConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("localhost");

                cfg.SetLoggerFactory(provider.GetService<ILoggerFactory>());

                cfg.ReceiveEndpoint("students_email_test", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.Durable = true;
                    e.AutoDelete = false;

                    e.Consumer<EmailConsumer>(provider);
                    EndpointConvention.Map<IEmailSend>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<IEmailSend>());

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusService>();

            services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IPracticRepository, PracticRepository>();
            services.AddScoped<IStudRepository, StudRepository>();
            services.AddScoped<IReportGen, ReportGenRepository>();

            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IPracticService, PracticService>();
            services.AddScoped<IStudService, StudService>();
            services.AddScoped<IUserService, UsersService>();
            services.AddScoped<IReportService, ReportGeneration>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailSender>();

            services.AddEntityFrameworkNpgsql().AddDbContext<StudentAccountingContext>(optionsAction => optionsAction.UseNpgsql(Configuration.GetConnectionString("StudentAccounting")));

            services.AddIdentity<UsersEntityModel, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            })
                .AddEntityFrameworkStores<StudentAccountingContext>();
        }

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
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                opt.RoutePrefix = String.Empty;
            });

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
