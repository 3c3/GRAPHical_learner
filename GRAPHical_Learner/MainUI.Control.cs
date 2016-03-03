using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {
        void EnablePhysics()
        {
            fs.SetForce(10.0f);
            fs.Reset();
            physics = true;
            physBtn.Text = "Изключи физика";
        }

        void DisablePhysics()
        {
            physics = false;
            physBtn.Text = "Включи физика";
        }

        void EnableEdgeAdding()
        {
            addEdgeEnabled = true;
            edgeBtn.Text = "Добавяне на ребра: включено";
        }

        void DisableEdgeAdding()
        {
            addEdgeEnabled = false;
            edgeBtn.Text = "Добавяне на ребра: изключено";
        }

        void ChangeGraph(Graph newGraph)
        {
            activeGraph = newGraph;
            fs.SetGraph(activeGraph);
        }

        void ClearAll()
        {
            Vertex.ResetCounter();
            Edge.ResetCounter();
            ChangeGraph(new Graph());
            DisablePhysics();
            DisableEdgeAdding();
        }

        void CenterGraph()
        {
            float minX = 99999.0f, maxX = 0f, minY = 9999999.0f, maxY = 0;
            foreach (Vertex v in activeGraph.vertices)
            {
                if (v.edges.Count == 0) continue;
                if (v.x < minX) minX = v.x;
                if (v.x > maxX) maxX = v.x;
                if (v.y < minY) minY = v.y;
                if (v.y > maxY) maxY = v.y;
            }

            float w = maxX - minX;
            float h = maxY - minY;

            renderFrame.xCenter = minX + w / 2.0f;
            renderFrame.yCenter = minY + h / 2.0f;
        }
    }
}
