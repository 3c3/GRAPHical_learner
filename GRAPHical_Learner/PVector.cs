using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// 2D вектор, ползва се за алгоритъма за подреждане
    /// По-удобен от SFML-ския
    /// </summary>
    public class PVector
    {
        public double x, y;
        
        public PVector()
        {
            x = 0;
            y = 0;
        }

        public PVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public void Add(PVector pv)
        {
            x += pv.x;
            y += pv.y;
        }

        public void Multiply(double k)
        {
            x *= k;
            y *= k;
        }

        public double CalcLengthSquared()
        {
            return x * x + y * y;
        }
    }
}
