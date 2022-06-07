using System;
using ImageDiceMap;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageClass image = new ImageClass("C:/Users/Hybrid/Desktop/test images/1/1/1.jpg");

            var dArray = image.DiceArray;

            
            for (int i = 0; i < image.Height/3; i++)
            {
                for (int j = 0; j < image.Width/3; j++)
                {
                        Console.Write(dArray[i,j] + "");
                }
                Console.WriteLine();
            }

            var eArray = image.EncodedArray;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (eArray[i, j] == ImageClass.Pixels.Black)
                    Console.Write(" ");
                    else if (eArray[i, j] == ImageClass.Pixels.White)
                    Console.Write(".");
                    
                }
                Console.WriteLine();
            }

        }

    }
}
