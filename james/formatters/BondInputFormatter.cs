namespace james.Formatters
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bond;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public abstract class BondInputFormatter<TReader> : InputFormatter
    {
        private readonly ConcurrentDictionary<Type, Deserializer<TReader>> _deserializers = new ConcurrentDictionary<Type, Deserializer<TReader>>();

        protected override bool CanReadType(Type type)
        {
            return typeof(IBonded).IsAssignableFrom(type) ||
                   type.GetTypeInfo().GetCustomAttribute<SchemaAttribute>() != null;
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;
            var type = context.ModelType;
            var stream = request.Body;

            if (stream.Length == 0)
            {
                context.ModelState.AddModelError(
                    context.ModelName,
                    new ArgumentNullException(context.ModelName, "No body was found."),
                    context.Metadata);

                return await InputFormatterResult.FailureAsync();
            }

            try
            {
                object value;
                if (typeof(IBonded).IsAssignableFrom(type) &&
                    type.GetTypeInfo().IsGenericType)
                {
                    value = await this.ReadFromStreamBondedAsync(stream, type.GenericTypeArguments[0]);
                }
                else
                {
                    value = await this.ReadFromStreamAsync(stream, type);
                }

                return await InputFormatterResult.SuccessAsync(value);
            }
            catch (InvalidDataException ex)
            {
                context.ModelState.AddModelError(
                    context.ModelName,
                    new ArgumentOutOfRangeException(context.ModelName, ex.Message),
                    context.Metadata);

                return await InputFormatterResult.FailureAsync();
            }
        }

        protected abstract Task<TReader> CreateReaderAsync(Stream stream);

        protected Deserializer<TReader> GetOrCreateDeserializer(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return this._deserializers.GetOrAdd(type, t => new Deserializer<TReader>(t));
        }

        protected async virtual Task<object> ReadFromStreamAsync(Stream stream, Type type)
        {
            var deserializer = this.GetOrCreateDeserializer(type);
            var reader = await this.CreateReaderAsync(stream);
            return deserializer.Deserialize(reader);
        }

        protected async virtual Task<object> ReadFromStreamBondedAsync(Stream stream, Type baseType)
        {
            var reader = await this.CreateReaderAsync(stream);
            var bondedType = typeof(Bonded<,>).MakeGenericType(baseType, typeof(TReader));
            return Activator.CreateInstance(bondedType, reader);
        }
    }
}