using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace GRAPHical_Learner
{
    public class Circle : IDrawable, IMovable
    {
        public Vector2f center;
        float radius;
        private Color color;

        public Circle(float x, float y, float r)
        {
            center = new Vector2f(x, y);
            radius = r;
            color = Color.White;
        }

        public Circle(float x, float y, float r, Color c)
        {
            center = new Vector2f(x, y);
            radius = r;
            color = c;
        }

        public List<Drawable> getDrawables(RenderFrame rf)
        {
            Vector2f actualPos = new Vector2f(center.X - rf.xCenter, center.Y - rf.yCenter); // преместване на камерата
            actualPos.X *= rf.zoom;
            actualPos.Y *= rf.zoom;
            float actualR = radius * rf.zoom;

            // превръщане в екранни координати

            actualPos.X += rf.width / 2;
            actualPos.Y = -actualPos.Y + rf.height / 2;

            actualPos.X -= radius*rf.zoom;
            actualPos.Y -= radius*rf.zoom;

            CircleShape cs = new CircleShape(actualR, 60);
            cs.Position = actualPos;
            cs.FillColor = color;

            return new List<Drawable> { cs };
        }

        public bool isInside(Vector2f point)
        {
            float dx = point.X - center.X;
            float dy = point.Y - center.Y;
            float d = dx * dx + dy * dy;
            return d <= radius * radius;
        }

        public void moveX(float dx)
        {
            center.X += dx;
        }

        public void moveY(float dy)
        {
            center.Y += dy;
        }
    }
}
