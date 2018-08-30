namespace james.Formatters
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;
    using Bond;

    public sealed class BondJsonOutputFormatter : BondOutputFormatter<SimpleJsonWriter>
    {
        public BondJsonOutputFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        protected override Task WriteToStreamAsync(Stream stream, object value)
        {
            var serializer = this.GetOrCreateSerializer(value);
            var sjw = new SimpleJsonWriter(stream);
            serializer.Serialize(value, sjw);
            sjw.Flush();
            return Task.CompletedTask;
        }

        protected override Task WriteToStreamBondedAsync(Stream stream, IBonded value)
        {
            var sjw = new SimpleJsonWriter(stream);
            value.Serialize(sjw);
            sjw.Flush();
            return Task.CompletedTask;
        }
    }
}