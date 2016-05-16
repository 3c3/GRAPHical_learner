using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Color3b
    {
        public byte red, green, blue;
        public Color3b(byte r, byte g, byte b)
        {
            red = r;
            green = g;
            blue = b;
        }

        public override bool Equals(object obj)
        {
            if(obj is Color3b)
            {
                Color3b otherColor = (Color3b)obj;
                if (red != otherColor.red) return false;
                if (green != otherColor.green) return false;
                if (blue != otherColor.blue) return false;
                return true;
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return String.Format("{0};{1};{2}", red, green, blue);
        }
    }
}
