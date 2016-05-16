using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Проста линийка за рисуване
    /// </summary>
    public class Line
    {
        private static float margin = 15;

        public void SetSource(float x, float y)
        {
            va[0].Position.X = x;
            va[0].Position.Y = y;
        }

        public void SetDestination(float x, float y)
        {
            va[1].Position.X = x;
            va[1].Position.Y = y;
        }

        public void SetColor(Color3b c3b)
        {
            Color color = new Color(c3b.red, c3b.green, c3b.blue);
            SetColor(color);
        }

        public void SetColor(Color color)
        {
            va[0].Color = color;
            va[1].Color = color;
            va2[0].Color = color;
            va2[1].Color = color;
        }

        public void Rotate(float angle)
        {
            float cosA = (float)Math.Cos(angle);
            float sinA = (float)Math.Sin(angle);

            float x0 = va[1].Position.X - va[0].Position.X;
            float y0 = va[1].Position.Y - va[0].Position.Y;

            float x = x0 * cosA - y0 * sinA;
            float y = x0 * sinA + y0 * cosA;

            va[1].Position.X = x + va[0].Position.X;
            va[1].Position.Y = y + va[0].Position.Y;
        }

        public bool HitCheck(Vector2f globalPos)
        {
            double x1 = va[0].Position.X;
            double y1 = va[0].Position.Y;

            double x2 = va[1].Position.X;
            double y2 = va[1].Position.Y;

            double x3 = globalPos.X;
            double y3 = globalPos.Y;

            double xRight, xLeft, yTop, yBottom;

            if(x1>x2)
            {
                xRight = x1;
                xLeft = x2;
            }
            else
            {
                xRight = x2;
                xLeft = x1;
            }

            if(y1 > y2)
            {
                yTop = y1;
                yBottom = y2;
            }
            else
            {
                yTop = y2;
                yBottom = y1;
            }

            if (x3 - xRight > margin) return false;
            if (xLeft - x3 > margin) return false;
            if (y3 - yTop > margin) return false;
            if (yBottom - y3 > margin) return false;

            double s = Math.Abs(x1 * y2 + y1 * x3 + x2 * y3 - x3 * y2 - y3 * x1 - x2 * y1);
         
            double d = GetLength();
            double h = s / d;

            /*bool result = h <= margin;
            if(result)
            {
                //Console.WriteLine("x1: {0}, y1: {1}, x2: {2}, y2:{3}", x1, y1, x2, y2);
                Console.WriteLine("S: {0}, d: {1}, h: {2}", s, d, h);
            }*/

            return h <= margin;
        }

        private float GetLength()
        {
            float dx = va[0].Position.X - va[1].Position.X;
            float dy = va[0].Position.Y - va[1].Position.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void GetTrigs(out float sinA, out float cosA)
        {
            float dx = va[1].Position.X - va[0].Position.X;
            float dy = va[1].Position.Y - va[0].Position.Y;

            float d = (float)Math.Sqrt(dx * dx + dy * dy);
            sinA = dy / d;
            cosA = dx / d;
        }

        SFML.Graphics.Vertex[] va = { new SFML.Graphics.Vertex(new Vector2f(), new Color(255, 0, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(), new Color(255, 0, 255, 127))};

        SFML.Graphics.Vertex[] va2 = new SFML.Graphics.Vertex[2];

        public void DrawSelf(RenderTarget window, RenderFrame rf)
        {
            va2[0] = va[0];
            va2[0].Position.X -= rf.xCenter;
            va2[0].Position.Y = rf.yCenter - va[0].Position.Y;
            va2[0].Position.X *= rf.zoom;
            va2[0].Position.Y *= rf.zoom;

            va2[0].Position.X += rf.width / 2;
            va2[0].Position.Y = va2[0].Position.Y + rf.height / 2;

            va2[1] = va[1];
            va2[1].Position.X -= rf.xCenter;
            va2[1].Position.Y = rf.yCenter - va[1].Position.Y;
            va2[1].Position.X *= rf.zoom;
            va2[1].Position.Y *= rf.zoom;


            va2[1].Position.X += rf.width / 2;
            va2[1].Position.Y = va2[1].Position.Y + rf.height / 2;


            window.Draw(va2, PrimitiveType.Lines);
        }
    }
}
