using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public partial class MainUI
    {// тук са всичките контролни функции на основния интерфейс

        /// <summary>
        /// Включва физиката, нулира брояча на енергия, задава първоначална сила и сменя текста на бутона
        /// </summary>
        void EnablePhysics()
        {
            fs.SetForce(10.0f);
            fs.Reset();
            physics = true;
            physBtn.Text = "Изключи физика";
        }

        /// <summary>
        /// Изключва физиката и сменя текста на бутона
        /// </summary>
        void DisablePhysics()
        {
            physics = false;
            physBtn.Text = "Включи физика";
        }

        /// <summary>
        /// Включва добавянето на ребра
        /// </summary>
        void EnableEdgeAdding()
        {
            addEdgeEnabled = true;
            edgeBtn.Text = "Добавяне на ребра: включено";
        }

        /// <summary>
        /// Изключва добавянето на ребра
        /// </summary>
        void DisableEdgeAdding()
        {
            addEdgeEnabled = false;
            edgeBtn.Text = "Добавяне на ребра: изключено";
        }

        /// <summary>
        /// Сменя текущия граф с нов
        /// </summary>
        /// <param name="newGraph"></param>
        void ChangeGraph(Graph newGraph)
        {
            lastClickedVertex = null;
            activeGraph = newGraph;
            fs.SetGraph(activeGraph);
        }

        /// <summary>
        /// Изчиства абсолютно всичко
        /// </summary>
        void ClearAll()
        {
            lastClickedVertex = null;
            Vertex.ResetCounter();
            Edge.ResetCounter();
            ChangeGraph(new Graph());
            DisablePhysics();
            DisableEdgeAdding();
        }

        /// <summary>
        /// Центрира свързаните върхове на графа
        /// </summary>
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
