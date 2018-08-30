namespace james.Formatters
{
    using Bond.Protocols;
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;

    public sealed class BondCompactBinaryOutputFormatter : BondBinaryOutputFormatter<CompactBinaryWriter<OutputBuffer>>
    {
        public BondCompactBinaryOutputFormatter()
        {
            this.Writer = new CompactBinaryWriter<OutputBuffer>(this.Buffer);
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+compactbinary"));
        }
    }
}