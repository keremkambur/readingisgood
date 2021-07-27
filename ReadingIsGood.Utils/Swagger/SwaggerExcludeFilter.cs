using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReadingIsGood.Utils.Swagger
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        #region ISchemaFilter Members

        //public void Apply(Schema model, SchemaFilterContext context)
        //{
        //    if (model?.Properties == null || context.SystemType == null)
        //    {
        //        return;
        //    }

        //    var excludedProperties = context
        //            .SystemType
        //            .GetProperties()
        //            .Select(p => p.GetCustomAttribute<SwaggerExcludeAttribute>()?.JsonValue)
        //            .Where(p => p != null)
        //        ;

        //    foreach (var excludedProperty in excludedProperties)
        //    {
        //        if (model.Properties.ContainsKey(excludedProperty))
        //        {
        //            model.Properties.Remove(excludedProperty);
        //        }
        //    }
        //}

        #endregion

        #region Implementation of ISchemaFilter

        /// <inheritdoc />
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
        }

        #endregion
    }
}