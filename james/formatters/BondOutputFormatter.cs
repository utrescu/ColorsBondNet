
namespace james.Formatters
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bond;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public abstract class BondOutputFormatter<TWriter> : OutputFormatter
    {
        private readonly ConcurrentDictionary<Type, Serializer<TWriter>> _serializers = new ConcurrentDictionary<Type, Serializer<TWriter>>();

        protected sealed override bool CanWriteType(Type type)
        {
            return typeof(IBonded).IsAssignableFrom(type) ||
                   type.GetTypeInfo().GetCustomAttribute<SchemaAttribute>() != null;
        }

        public sealed override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Object != null)
            {
                if (typeof(IBonded).IsAssignableFrom(context.ObjectType))
                {
                    return this.WriteToStreamBondedAsync(context.HttpContext.Response.Body, (IBonded)context.Object);
                }

                return this.WriteToStreamAsync(context.HttpContext.Response.Body, context.Object);
            }

            // TODO: Should this be a throw for not writing anything when the object is null?
            return Task.CompletedTask;
        }

        protected Serializer<TWriter> GetOrCreateSerializer(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return this._serializers.GetOrAdd(value.GetType(), t => new Serializer<TWriter>(t));
        }

        protected abstract Task WriteToStreamAsync(Stream stream, object value);

        protected abstract Task WriteToStreamBondedAsync(Stream stream, IBonded value);
    }
}