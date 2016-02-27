using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Edge
    {
        public Vertex source, destination;
        public List<Property> properties;

        public Line line = new Line();

        public Edge(Vertex src, Vertex dest)
        {
            source = src;
            destination = dest;
        }

        public void DrawSelf(RenderWindow window, RenderFrame rf)
        {
            line.SetDestination(destination.x, destination.y);
            line.SetSource(source.x, source.y);
            line.DrawSelf(window, rf);
        }
    }
}
