namespace james.Formatters
{
    using System.IO;
    using System.Threading.Tasks;
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public sealed class BondSimpleBinaryInputFormatter : BondBinaryInputFormatter<SimpleBinaryReader<InputBuffer>>
    {
        public BondSimpleBinaryInputFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+simplebinary"));
        }

        protected async override Task<SimpleBinaryReader<InputBuffer>> CreateReaderAsync(Stream stream)
        {
            var buffer = await this.GetBufferFromStreamAsync(stream);
            return new SimpleBinaryReader<InputBuffer>(buffer);
        }
    }
}