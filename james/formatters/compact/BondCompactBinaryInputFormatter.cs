namespace james.Formatters
{
    using System.IO;
    using System.Threading.Tasks;
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public sealed class BondCompactBinaryInputFormatter : BondBinaryInputFormatter<CompactBinaryReader<InputBuffer>>
    {
        public BondCompactBinaryInputFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+compactbinary"));
        }

        protected async override Task<CompactBinaryReader<InputBuffer>> CreateReaderAsync(Stream stream)
        {
            var buffer = await this.GetBufferFromStreamAsync(stream);
            return new CompactBinaryReader<InputBuffer>(buffer);
        }
    }
}