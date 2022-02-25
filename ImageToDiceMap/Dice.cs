using System;
using System.Collections.Generic;
using System.Text;

namespace ImageDiceMap
{
    public class Dice
    {
        public enum Clr { White, Black }
        public enum Ornt { None, Vertical, Horizontal, Digonal, Antidiagonal }

        public Clr Background { get; set; }

        public int Number { get; set; }

        public Ornt Orientation { get; set; }


        public Dice(Clr background, int number, Ornt orientation)
        {
            Background = background;
            Number = number;
            Orientation = orientation;
        }

        public override string ToString()
        {
            string s = "";

            if (Background == Clr.Black)
                s += "B-";
            else
                s += "W-";

            s += Number.ToString() + "-";

            if (Orientation == Ornt.None)
                s += "N";
            else if (Orientation == Ornt.Vertical)
                s += "V";
            else if (Orientation == Ornt.Horizontal)
                s += "H";
            else if (Orientation == Ornt.Digonal)
                s += "D";
            else if (Orientation == Ornt.Antidiagonal)
                s += "A";

            return s;
        }
    }
}
