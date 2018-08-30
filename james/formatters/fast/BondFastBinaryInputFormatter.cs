namespace james.Formatters
{
    using System.IO;
    using System.Threading.Tasks;
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public sealed class BondFastBinaryInputFormatter : BondBinaryInputFormatter<FastBinaryReader<InputBuffer>>
    {
        public BondFastBinaryInputFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+fastbinary"));
        }

        protected async override Task<FastBinaryReader<InputBuffer>> CreateReaderAsync(Stream stream)
        {
            var buffer = await this.GetBufferFromStreamAsync(stream);
            return new FastBinaryReader<InputBuffer>(buffer);
        }
    }
}