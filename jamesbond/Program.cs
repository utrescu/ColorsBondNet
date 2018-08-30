using System;
using System.Net;
using System.Net.Http;
using Bond;
using Bond.IO.Unsafe;
using Bond.Protocols;

namespace bond
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0 || args.Length > 1)
            {
                System.Console.WriteLine("S'ha d'entrar el color a cercar o l'ID.");
                System.Console.WriteLine("Ex. Programa vermell o Programa 12");
                return;
            }

            var param = args[0];

            HttpClient client = new HttpClient();
            var x = client.GetAsync("http://localhost:5000/colors/" + param);
            HttpResponseMessage response = x.Result;
            var contens = response.Content.ReadAsByteArrayAsync();
            System.Console.WriteLine($"Result {response.StatusCode}");

            var input = new Bond.IO.Unsafe.InputBuffer(contens.Result);
            var reader = new CompactBinaryReader<InputBuffer>(input);
            if (response.StatusCode == HttpStatusCode.OK)
            {

                var dst = Deserialize<Colors.Color>.From(reader);
                System.Console.WriteLine($"{dst.Nom} -> {dst.Rgb}");

            }
            else
            {

                var dst = Deserialize<Colors.NoColor>.From(reader);
                System.Console.WriteLine($"Error {dst.Message}");

            }
        }
    }
}
