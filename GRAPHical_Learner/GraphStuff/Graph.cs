using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class Graph
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Edge> edges = new List<Edge>();

        public void DrawSelf(RenderWindow window, RenderFrame rf)
        {
            edges.ForEach(e => e.DrawSelf(window, rf));
            vertices.ForEach(v => v.DrawSelf(window, rf));
        }

        public Edge AddEdge(int idx1, int idx2)
        {
            Edge e = new Edge(vertices[idx1], vertices[idx2]);
            edges.Add(e);
            vertices[idx1].edges.Add(e);
            vertices[idx2].edges.Add(e);
            return e;
        }

        public int AddEdge(Vertex v1, Vertex v2)
        {
            Edge e = new Edge(v1, v2);
            edges.Add(e);
            v1.edges.Add(e);
            v2.edges.Add(e);
            return e.id;
        }

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
        }

        public void RemoveVertex(Vertex v)
        {
            List<Edge> remove = new List<Edge>();
            foreach(Edge e in edges)
            {
                if (e.destination == v || e.source == v) remove.Add(e);
            }

            foreach (Edge e in remove) RemoveEdge(e);
            vertices.Remove(v);
        }

        public void RemoveEdge(Edge e)
        {
            e.source.edges.Remove(e);
            e.destination.edges.Remove(e);
            edges.Remove(e);
        }

        public void ArrangeInCircle()
        {
            int nv = vertices.Count;
            float r = (nv * 150.0f) / (2 * (float)Math.PI);
            for (int i = 0; i < nv; i++)
            {
                Vertex v = vertices[i];
                float angle = ((float)i / (float)nv) * 2 * (float)Math.PI;
                v.x = (float)Math.Cos(angle) * r;
                v.y = (float)Math.Sin(angle) * r;
            }
        }
    }
}
