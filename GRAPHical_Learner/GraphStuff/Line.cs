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
            float x = globalPos.X;
            float y = globalPos.Y;

            if(va[0].Position.X < va[1].Position.X)
            {

            }
            return false;
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
