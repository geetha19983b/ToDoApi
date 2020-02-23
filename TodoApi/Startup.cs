using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Todo.Business;

using Todo.Repository;
//kfka
using HostedServices = Microsoft.Extensions.Hosting;

namespace TodoApi
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
            services.AddControllers();
            services.AddScoped<IBusiness, Business>();
            // services.AddScoped<IRepository, Repository>();
            //services.AddScoped<IRepository, SqlRepository>();

            //services.AddDbContext<AuthorContext>(options =>
            //{
            //    options.UseSqlServer(
            //       Configuration.GetConnectionString("SqlDatabase"));
            //});

            // requires using Microsoft.Extensions.Options
            services.Configure<MongoDbSettings>(
                Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddScoped<IRepository, MongoRepository>();

            //kafka
            //services.AddSingleton<HostedServices.IHostedService, ProcessOrdersService>();
            var producerConfig = new ProducerConfig();
            var consumerConfig = new ConsumerConfig();
            Configuration.Bind("producer", producerConfig);
            Configuration.Bind("consumer", consumerConfig);

            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddSingleton<ConsumerConfig>(consumerConfig);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
