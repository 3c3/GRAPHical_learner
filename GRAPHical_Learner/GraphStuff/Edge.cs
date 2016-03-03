using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Edge : PropertyHolder
    {
        private static int idCounter = 0;

        public Vertex source, destination;
        public readonly int id;

        public Line line = new Line();

        public Edge(Vertex src, Vertex dest)
        {
            id = idCounter++;
            source = src;
            destination = dest;
        }

        public override string GetName()
        {
            return String.Format("Ребро {0}-{1}({2})", source.id, destination.id, id);
        }

        public void DrawSelf(RenderWindow window, RenderFrame rf)
        {
            line.SetDestination(destination.x, destination.y);
            line.SetSource(source.x, source.y);
            line.DrawSelf(window, rf);
        }

        public static void ResetCounter()
        {
            idCounter = 0;
        }
    }
}
