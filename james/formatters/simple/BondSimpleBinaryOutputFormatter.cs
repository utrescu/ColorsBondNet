namespace james.Formatters
{
    using Bond.IO.Safe;
    using Microsoft.Net.Http.Headers;
    using Bond.Protocols;

    public class BondSimpleBinaryOutputFormatter : BondBinaryOutputFormatter<SimpleBinaryWriter<OutputBuffer>>
    {
        public BondSimpleBinaryOutputFormatter()
        {
            this.Writer = new SimpleBinaryWriter<OutputBuffer>(this.Buffer);
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.microsoft.bond+simplebinary"));
        }
    }
}