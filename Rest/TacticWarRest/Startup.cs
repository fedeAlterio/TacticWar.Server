using Microsoft.OpenApi.Models;
using MediatR;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using TacticWar.Rest.Middlewares;
using TacticWar.Rest.ViewModels.Services;
using TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace TacticWar.Rest
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
            services.AddOptions();

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            //services.AddInMemoryRateLimiting();
            //services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddMediatR(typeof(Startup));
            services.AddScoped<ServiceFactory>(p => p.GetService);

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RisikoRest", Version = "v1" });
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });

            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});

            AddServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseIpRateLimiting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RisikoRest v1"));

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", () => Results.Redirect("/index.html"));
                endpoints.MapControllers();
            });


            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings.Remove(".data");
            provider.Mappings[".data"] = "application/octet-stream";
            provider.Mappings.Remove(".wasm");
            provider.Mappings[".wasm"] = "application/wasm";
            provider.Mappings.Remove(".symbols.json");
            provider.Mappings[".symbols.json"] = "application/octet-stream";
            provider.Mappings.Remove(".unityweb");
            provider.Mappings.Add(".unityweb", "application/octet-stream");

            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });

            //AddSpaConfiguration(app, env, provider);
        }

        private static void AddSpaConfiguration(IApplicationBuilder app, IWebHostEnvironment env, FileExtensionContentTypeProvider provider)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "ClientApp", "dist", "assets")),
                RequestPath = "/assets",
                ContentTypeProvider = provider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "ClientApp", "dist")),
                RequestPath = "",
                ContentTypeProvider = provider
            });
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }


            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                //    spa.UseAngularCliServer(npmScript: "start");
                //    // spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                //}
            });
        }


        // Utils
        private void AddServices(IServiceCollection services)
        {
            services.AddTacticWar();
            services.AddSingleton<IViewModelsLocator, ViewModelsLocator>();
        }
    }
}
