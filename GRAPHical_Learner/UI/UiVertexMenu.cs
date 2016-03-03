using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class UiVertexMenu : UiPanel
    {
        private UiVerticalMenu topMenu, bottomMenu;
        private Graph graph;
        private Vertex v;

        Dictionary<int, Edge> dict = new Dictionary<int, Edge>();

        public UiVertexMenu(Vertex v, Graph g, float x, float y)
        {
            visible = true;
            children = new List<UiComponent>();

            X = (int)x;
            Y = (int)y;

            this.v = v;
            graph = g;
            topMenu = new UiVerticalMenu();
            topMenu.AddItem("Премахни връх", RemoveVertex);

            topMenu.X = 3;
            topMenu.Y = 3;

            Width = topMenu.Width + 6;
            Height = topMenu.Height + 6;

            AddChild(topMenu);

            int yp = topMenu.Height + 6; 

            if(v.edges.Count > 0)
            {
                UiLabel label = new UiLabel("Премахни ребро:", GraphicScheme.font1);
                label.X = 3;
                label.Y = yp;
                yp += label.Height + 3;
                AddChild(label);

                bottomMenu = new UiVerticalMenu();

                bottomMenu.X = 3;
                bottomMenu.Y = yp;

                bottomMenu.Width = topMenu.Width;

                Edge e;
                Vertex vo;                

                for (int i = 0; i < v.edges.Count; i++)
                {
                    e = v.edges[i];
                    vo = e.source == v ? e.destination : e.source;
                    dict.Add(bottomMenu.AddItem(vo.id.ToString(), RemoveEdge).id, e);
                }

                AddChild(bottomMenu);
                yp += bottomMenu.Height;

                Height = yp + 3;
            }
            
        }

        void RemoveVertex(UiComponent sender, Object args)
        {
            graph.RemoveVertex(v);
            Remove();
        }

        void RemoveEdge(UiComponent sender, Object args)
        {
            int id = (int)args;
            Console.WriteLine(id);
            Edge e = dict[id];
            graph.RemoveEdge(e);
            Remove();
        }
    }
}
