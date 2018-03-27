using System;
using System.Collections.Generic;
using System.Linq;
////using System.Text;
using System.Threading.Tasks;

namespace Aiv.Mpg123.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string decoder in Mpg123.Decoders)
            {
                Console.WriteLine(decoder);
            }

            Out123 out123 = new Out123();

            out123.Open();

            Mpg123 mpg123 = new Mpg123();
            mpg123.Open("Assets/bensound-epic.mp3");

            int offset = 0;
            while(true)
            {
                byte[] data = null;
                uint size = 0;
                Mpg123.Errors error = mpg123.DecodeFrame(ref offset, ref data, ref size);
                if (error == Mpg123.Errors.NEW_FORMAT)
                {
                    long rate = 0;
                    int channels = 0;
                    int encoding = 0;
                    mpg123.GetFormat(ref rate, ref channels, ref encoding);
                    out123.Start(rate, channels, encoding);
                }
                else if (error == Mpg123.Errors.OK)
                {
                    out123.Play(data);
                }
            }

            Console.ReadLine();
        }
    }
}
