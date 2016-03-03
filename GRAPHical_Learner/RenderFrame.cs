using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Рамка за рисуване
    /// Определя каква и коя част от глобалното пространство се вижда 
    /// </summary>
    public class RenderFrame : IMovable
    {
        public float xCenter, yCenter;
        public float scale;

        public float zoom;

        public uint width, height;

        public RenderFrame()
        {
            width = 1024;
            height = 600;
            xCenter = 0;
            yCenter = 0;
            scale = 0;
        }

        public void CalcZoom() // по-интуитивен зуум
        {
            zoom = (float)Math.Pow(4, scale);
        }

        public void MoveX(float dx)
        {
            xCenter -= dx;
        }

        public void MoveY(float dy)
        {
            yCenter -= dy;
        }
    }
}
