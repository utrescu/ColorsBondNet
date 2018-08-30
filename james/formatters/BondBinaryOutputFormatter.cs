namespace james.Formatters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Bond;
    using Bond.IO.Safe;

    public abstract class BondBinaryOutputFormatter<TWriter> : BondOutputFormatter<TWriter>
    {
        protected OutputBuffer Buffer { get; } = new OutputBuffer();

        protected TWriter Writer { get; set; }

        protected ArraySegment<byte> SerializeToBytes(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Buffer.Position = 0;
            var serializer = this.GetOrCreateSerializer(value);
            serializer.Serialize(value, this.Writer);
            return this.Buffer.Data;
        }

        protected ArraySegment<byte> SerializeToBytes(IBonded value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Buffer.Position = 0;
            value.Serialize(this.Writer);
            return this.Buffer.Data;
        }

        protected sealed override Task WriteToStreamAsync(Stream stream, object value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bytes = this.SerializeToBytes(value);
            return stream.WriteAsync(bytes.Array, bytes.Offset, bytes.Count);
        }

        protected sealed override Task WriteToStreamBondedAsync(Stream stream, IBonded value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var bytes = this.SerializeToBytes(value);
            return stream.WriteAsync(bytes.Array, bytes.Offset, bytes.Count);
        }
    }
}