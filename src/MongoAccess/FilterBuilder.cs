using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoAccess
{
    public class FilterBuilder<T> : IFilterBuilder<T>
    {
        private readonly FilterDefinitionBuilder<T> builder;
        private readonly List<FilterDefinition<T>> filters;

        public FilterBuilder()
        {
            filters = new();
            builder = Builders<T>.Filter;
        }

        public FilterDefinition<T> BuildFilter(T definition)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));

            foreach (PropertyInfo propertyInfo in definition.GetType().GetProperties())
                ExtractFilter(definition, propertyInfo);

            return builder.And(filters);
        }

        private void ExtractFilter(T definition, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.Name == nameof(DateTime)) return;

            var property = propertyInfo.GetValue(definition);

            if (property == null) return;

            var filter = builder?.Eq(propertyInfo.Name, property);

            if (filter == null) return;

            filters?.Add(filter);
        }
    }
}
