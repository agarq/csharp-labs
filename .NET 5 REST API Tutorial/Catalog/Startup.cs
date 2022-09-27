using Catalog.Repositories;
using Catalog.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;

namespace Catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //here we register the services we will use accross the application
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            //to register the injection of the database
            services.AddSingleton<IMongoClient>(serviceProvider => 
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            //v1 to register the dependency injection on the In memory repository
            //services.AddSingleton<IItemsRepository, InMemItemsRepository>(); 

            //v2 now we will have the repo in the MongoDb Database
            services.AddSingleton<IItemsRepository, MongoDbItemsRepository>(); 

            //we add the 
            services.AddControllers(options => 
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
            });

            //adding the healthchecks
            services.AddHealthChecks()
                .AddMongoDb(
                    mongoDbSettings.ConnectionString, 
                    name: "mongodb", 
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new[] { "ready" }); //we check if we can reach the database
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //this is for ready
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions{
                    //predicate is to filter which healthchecks we wanna include
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = async(context, report) => 
                    {
                        var result = JsonSerializer.Serialize(
                            new{
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(entry => new{
                                    name = entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                    duration = entry.Value.Duration.ToString()
                                })
                            }
                        );

                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });

                //this is for live
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions{
                    //this just give us the response of a ping, discarding the service dependencies and just checking if the app service is alive
                    Predicate = (_) => false 
                }); 
            });
        }
    }
}
