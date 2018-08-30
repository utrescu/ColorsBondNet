namespace james.Formatters
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Threading.Tasks;
    using Bond;
    using Bond.IO.Safe;

    public abstract class BondBinaryInputFormatter<TReader> : BondInputFormatter<TReader>
    {
        protected async Task<InputBuffer> GetBufferFromStreamAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            // TODO: This is terrible to tie the buffer like this. Figure out how to do this with a stream when
            // bond.unsafe is supported for a Stream buffer.
            var rentedBytes = ArrayPool<byte>.Shared.Rent(64 * 1024);

            var mem = new MemoryStream(rentedBytes);
            await stream.CopyToAsync(mem);

            var bytes = new byte[stream.Position];
            Buffer.BlockCopy(rentedBytes, 0, bytes, 0, (int)stream.Position);
            ArrayPool<byte>.Shared.Return(rentedBytes);

            return new InputBuffer(bytes);
        }

        protected async sealed override Task<object> ReadFromStreamAsync(Stream stream, Type type)
        {
            var deserializer = this.GetOrCreateDeserializer(type);
            var reader = await this.CreateReaderAsync(stream);
            return deserializer.Deserialize(reader);
        }

        protected async sealed override Task<object> ReadFromStreamBondedAsync(Stream stream, Type baseType)
        {
            var reader = await this.CreateReaderAsync(stream);
            var bondedType = typeof(Bonded<,>).MakeGenericType(baseType, typeof(TReader));
            return Activator.CreateInstance(bondedType, reader);
        }
    }
}