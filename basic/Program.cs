using System;
using Bond;
using Bond.Protocols;
using Bond.IO.Safe;
using Colors;

namespace primer
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = new Color
            {
                Nom = "Vermell",
                Rgb = "#FF0000"
            };

            var output = new OutputBuffer();
            var writer = new CompactBinaryWriter<OutputBuffer>(output);

            // The first calls to Serialize.To and Deserialize<T>.From can take
            // a relatively long time because they generate the de/serializer
            // for a given type and protocol.
            Serialize.To(writer, src);

            var input = new InputBuffer(output.Data);
            var reader = new CompactBinaryReader<InputBuffer>(input);

            var dst = Deserialize<Color>.From(reader);
            System.Console.WriteLine($"{dst.Nom} - {dst.Rgb}");
        }
    }
}
