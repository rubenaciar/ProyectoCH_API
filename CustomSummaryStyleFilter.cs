    using Swashbuckle.AspNetCore.SwaggerGen;
    using Microsoft.OpenApi.Models;

namespace ProyectoFinalCoderHouse
{


    public class CustomSummaryStyleFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Modificar el estilo de la fuente en los comentarios <summary>
            operation.Summary = "<span font-size: 20px; font-family: Arial;'>" + operation.Summary + "</span>";

            //if (context.ApiDescription.RelativePath.Contains("/api/Usuario/{nombreUsuario}/{contraseña}"))
            //{
            //    operation.Summary = "<span style='font-size: 20px; font-family: Arial;'>" + operation.Summary + "</span>";
            //}
        }
    }
}
