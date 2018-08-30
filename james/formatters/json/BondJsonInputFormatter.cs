namespace james.Formatters
{
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public sealed class BondJsonInputFormatter : BondInputFormatter<SimpleJsonReader>
    {
        public BondJsonInputFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        protected override Task<SimpleJsonReader> CreateReaderAsync(Stream stream)
        {
            return Task.FromResult(new SimpleJsonReader(stream));
        }
    }
}