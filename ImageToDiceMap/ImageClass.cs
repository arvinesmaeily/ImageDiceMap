using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using static ImageDiceMap.Dice;

namespace ImageDiceMap
{
    public class ImageClass
    {
        public enum Pixels { Black, White }

        public Dictionary<int[,], string> Dices = new Dictionary<int[,], string>();

        public Bitmap Image { get; set; }
        public Color[,] ColorArray { get; set; }

        public Pixels[,] EncodedArray { get; set; }

        public Dice[,] DiceArray { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ImageClass(string path)
        {
            Image = new Bitmap(path);

            Width = Image.Width;
            Height = Image.Height;

            CropImage();
            Encode();
            ConvertToDiceMap();


        }

        private void CropImage()
        {
            int cropWidth = 0;
            int cropHeight = 0;

            if (Width % 3 == 0)
                cropWidth = Width;
            else if (Width % 3 == 1)
                cropWidth = Width - 1;
            else if (Width % 3 == 2)
                cropWidth = Width - 2;

            if (Height % 3 == 0)
                cropHeight = Height;
            else if (Height % 3 == 1)
                cropHeight = Height - 1;
            else if (Height % 3 == 2)
                cropHeight = Height - 2;

            Width = cropWidth;
            Height = cropHeight;

            ColorArray = new Color[Height, Width];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    ColorArray[j, i] = Image.GetPixel(i, j);
                }
            }

        }

        /// <summary>
        /// Encode given <see cref="ImageClass"/> to Black (0) and White (1) 2D <see cref="int"/> Array
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private void Encode()
        {


            Pixels[,] encodedArray = new Pixels[Height, Width];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var val = (ColorArray[j, i].R + ColorArray[j, i].G + ColorArray[j, i].B) / 3;

                    if (val >= 0 && val < 100)
                        encodedArray[j, i] = Pixels.Black;
                    else if (val >= 100 && val <= 256)
                        encodedArray[j, i] = Pixels.White;
                }

                EncodedArray = encodedArray;
            }

        }


        /// <summary>
        /// Convert Encoded 2D <see cref="int"/> Array Into 2D <see cref="Dice"/> Array
        /// </summary>
        private void ConvertToDiceMap()
        {

            int w_units = Width / 3;
            int h_units = Height / 3;

            Dice[,] diceArray = new Dice[h_units, w_units];

            for (int i = 0; i < w_units - 1; i++)
            {
                for (int j = 0; j < h_units - 1; j++)
                {
                    int i1 = j * 3;
                    int i2 = i1 + 1;
                    int i3 = i2 + 2;

                    int j1 = i * 3;
                    int j2 = j1 + 1;
                    int j3 = j2 + 2;
                    Pixels[,] scopeArray = new Pixels[3, 3] { { EncodedArray[i1, j1], EncodedArray[i1,j2], EncodedArray[i1, j3] },
                                                        { EncodedArray[i2, j1], EncodedArray[i2,j2], EncodedArray[i2, j3] },
                                                        { EncodedArray[i3, j1], EncodedArray[i3,j2], EncodedArray[i3, j3] }};
                    diceArray[j, i] = DetectDice(scopeArray);
                }
            }

            DiceArray = diceArray;
        }


        /// <summary>
        /// Detect and return The closest <see cref="Dice"/> to given 2D 3x3 <see cref="int"/> Array
        /// </summary>
        /// <param name="encodedArray"></param>
        /// <returns></returns>
        private Dice DetectDice(Pixels[,] encodedArray)
        {
            #region Dices
            Dictionary<Pixels[,], Dice> Dices = new Dictionary<Pixels[,], Dice>();

            //Black
            //0
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.Black, Pixels.Black, Pixels.Black} }, new Dice(Clr.Black, 0, Ornt.None));
            //1
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.Black, Pixels.Black, Pixels.Black} }, new Dice(Clr.Black, 1, Ornt.None));
            //2
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.Black, Pixels.White, Pixels.Black} }, new Dice(Clr.Black, 2, Ornt.Vertical));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.Black, Pixels.Black, Pixels.Black} }, new Dice(Clr.Black, 2, Ornt.Horizontal));
            //3
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.Black, Pixels.Black},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.Black, Pixels.Black, Pixels.White } }, new Dice(Clr.Black, 3, Ornt.Digonal));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.Black, Pixels.White},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.Black} }, new Dice(Clr.Black, 3, Ornt.Antidiagonal));
            //4
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.White} }, new Dice(Clr.Black, 4, Ornt.None));
            //5
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.White} }, new Dice(Clr.Black, 5, Ornt.None));
            //6
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.White, Pixels.Black, Pixels.White} }, new Dice(Clr.Black, 6, Ornt.Vertical));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.White, Pixels.White, Pixels.White} }, new Dice(Clr.Black, 6, Ornt.Horizontal));

            //White
            //0
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.White, Pixels.White, Pixels.White} }, new Dice(Clr.White, 0, Ornt.None));
            //1
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.White, Pixels.White },
                { Pixels.White, Pixels.Black, Pixels.White },
                { Pixels.White, Pixels.White, Pixels.White} }, new Dice(Clr.White, 1, Ornt.None));
            //2
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.White, Pixels.Black, Pixels.White}}, new Dice(Clr.White, 2, Ornt.Vertical));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.White, Pixels.White} }, new Dice(Clr.White, 2, Ornt.Horizontal));
            //3
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.White, Pixels.White},
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.White, Pixels.White, Pixels.Black} }, new Dice(Clr.White, 3, Ornt.Digonal));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.White, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.White },
                { Pixels.Black, Pixels.White, Pixels.White } }, new Dice(Clr.White, 3, Ornt.Antidiagonal));
            //4
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.White, Pixels.White},
                { Pixels.Black, Pixels.White, Pixels.Black} }, new Dice(Clr.White, 4, Ornt.None));
            //5
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.White, Pixels.Black, Pixels.White},
                { Pixels.Black, Pixels.White, Pixels.Black} }, new Dice(Clr.White, 5, Ornt.None));
            //6
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.Black, Pixels.White, Pixels.Black},
                { Pixels.Black, Pixels.White, Pixels.Black} }, new Dice(Clr.White, 6, Ornt.Vertical));
            Dices.Add(new Pixels[3, 3] {
                { Pixels.Black, Pixels.Black, Pixels.Black},
                { Pixels.White, Pixels.White, Pixels.White },
                { Pixels.Black, Pixels.Black, Pixels.Black} }, new Dice(Clr.White, 6, Ornt.Horizontal));
            #endregion

            Dictionary<Dice, int> similarity = new Dictionary<Dice, int>();

            foreach (var d in Dices.Keys)
            {
                int c = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (d[i, j] == encodedArray[i, j])
                        {
                            c++;
                        }
                    }
                }
                similarity.Add(Dices[d], c);

            }

            return similarity.OrderBy(x => x.Value).LastOrDefault().Key;

        }
    }
}