using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReadingIsGood.Utils.Swagger
{
    public class RemoveVersionFromParameter : IOperationFilter
    {

        #region Implementation of IOperationFilter

        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
            => operation?.Parameters?.Remove(operation.Parameters.SingleOrDefault(p => p.Name == "version"));

        #endregion
    }
}