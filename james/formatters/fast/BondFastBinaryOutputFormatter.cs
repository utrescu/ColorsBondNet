namespace james.Formatters
{
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public sealed class BondFastBinaryOutputFormatter : BondBinaryOutputFormatter<FastBinaryWriter<OutputBuffer>>
    {
        public BondFastBinaryOutputFormatter()
        {
            this.Writer = new FastBinaryWriter<OutputBuffer>(this.Buffer);
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+fastbinary"));
        }
    }
}