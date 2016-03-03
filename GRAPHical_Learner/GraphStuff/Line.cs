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

        SFML.Graphics.Vertex[] va = { new SFML.Graphics.Vertex(new Vector2f(), new Color(255, 0, 255, 127)), 
                                               new SFML.Graphics.Vertex(new Vector2f(), new Color(255, 0, 255, 127))};

        SFML.Graphics.Vertex[] va2 = new SFML.Graphics.Vertex[2];

        public void DrawSelf(RenderWindow window, RenderFrame rf)
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
