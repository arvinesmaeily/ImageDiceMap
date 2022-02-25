using System;
using ImageDiceMap;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageClass image = new ImageClass("");

            var dArray = image.DiceArray;

            
            for (int i = 0; i < image.Height/3; i++)
            {
                for (int j = 0; j < image.Width/3; j++)
                {
                    //if (dArray[i, j] == ImageClass.Pixels.Black)
                        //Console.Write(" ");
                    //else if (dArray[i, j] == ImageClass.Pixels.White)
                        //Console.Write(".");
                        Console.Write(dArray[i,j] + " ");
                }
                Console.WriteLine();
            }
            
        }

    }
}
