using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Vertex : IMovable
    {
        private static int idCounter = 0;
        public readonly int id;
        private ScalableLabel idLabel;

        public List<Property> properties;
        public List<Edge> edges = new List<Edge>();

        public float x, y;
        public PVector velocity = new PVector();

        public Circle circle, selectionCircle;

        public bool selected;

        public Vertex()
        {
            id = idCounter++;
            idLabel = new ScalableLabel(id.ToString());

            circle = new Circle(0, 0, 20);
            selectionCircle = new Circle(0, 0, 25, Color.Cyan);
            x = 0;
            y = 0;
        }

        public Vertex(float x, float y)
        {
            id = idCounter++;
            idLabel = new ScalableLabel(id.ToString());

            circle = new Circle(x, y, 20);
            selectionCircle = new Circle(0, 0, 25, Color.Cyan);
            this.x = x;
            this.y = y;
        }

        public void DrawSelf(RenderWindow window, RenderFrame rf)
        {
            circle.center.X = x;
            circle.center.Y = y;

            selectionCircle.center.X = x;
            selectionCircle.center.Y = y;

            List<Drawable> ld = new List<Drawable>();
            if (selected) ld.AddRange(selectionCircle.getDrawables(rf));
            ld.AddRange(circle.getDrawables(rf));
            ld.ForEach(d => window.Draw(d));

            idLabel.DrawSelf(window, rf, x, y);
        }

        public void MoveX(float dx)
        {
            x += dx;
            velocity.x = 0;
            velocity.y = 0;
        }

        public void MoveY(float dy)
        {
            y += dy;
            velocity.x = 0;
            velocity.y = 0;
        }

        public static void ResetCounter()
        {
            idCounter = 0;
        }
    }
}
