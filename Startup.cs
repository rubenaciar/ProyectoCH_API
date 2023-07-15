using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalCoderHouse.Repository;
using System.IO;
using System.Reflection;
using ProyectoFinalCoderHouse.EntityORM;

namespace ProyectoFinalCoderHouse
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

            services.AddCors(policy =>
            {
                policy.AddDefaultPolicy(options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                // Personalizar los estilos CSS para los comentarios <summary>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proyecto Final CH - Ruben Aciar", Version = "v1" });
                c.OperationFilter<CustomSummaryStyleFilter>();
                // Especificar la ruta del archivo XML de documentación
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            var ConnectionString = Configuration.GetConnectionString("connectionDB");
            //INYECCION DE DEPENDENCIAS PARA LA CONEXION A LA BASE DE DATOS Y CRUD
            services.AddDbContext<SistemaGestionContext>(options => options.UseSqlServer(ConnectionString));
            services.AddScoped<VentaHandler>();
            services.AddScoped<ProductoHandler>();
            services.AddScoped<UsuarioHandler>();
            services.AddScoped<ProductoVendidoHandler>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto Final CH - Ruben Aciar v1"));
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
