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
            physBtn.Text = "Физика(изключи)";
        }

        /// <summary>
        /// Изключва физиката и сменя текста на бутона
        /// </summary>
        void DisablePhysics()
        {
            physics = false;
            physBtn.Text = "Физика(включи)";
        }

        /// <summary>
        /// Включва добавянето на ребра
        /// </summary>
        void EnableEdgeAdding()
        {
            addEdgeEnabled = true;
            edgeBtn.Text = "Добави ребра(изключи)";
        }

        /// <summary>
        /// Изключва добавянето на ребра
        /// </summary>
        void DisableEdgeAdding()
        {
            addEdgeEnabled = false;
            edgeBtn.Text = "Добави ребра(включи)";
        }

        void DisableDirected()
        {
            activeGraph.Directed = false;
            directivityBtn.Text = "Насочен(не)";
        }

        void EnableDirected()
        {
            activeGraph.Directed = true;
            directivityBtn.Text = "Насочен(да)";
        }

        void DisableWeight()
        {
            weighted = false;
            weightBtn.Text = "Претеглен(не)";
        }

        void EnableWeight()
        {
            if(Property.EdgeWeightId == -1)
            {
                int propId = Property.GetPropertyId("тегло");
                Property.SetSpecialProperty(propId, SpecialProperty.EdgeWeight);
            }
            weighted = true;
            weightBtn.Text = "Претеглен(да)";
        }

        bool FinishWeight()
        {
            String val = (String)inputWeight.Value;
            int ival = 0;
            bool result = int.TryParse(val, out ival);
            if (result)
            {
                inputWeight.Value = ival;
                inputWeight = null;
                inputEdge = null;
                edgeInputLabel.visible = false;
                return true;
            }

            double dval = 0;
            result = double.TryParse(val, out dval);
            if (result)
            {
                inputWeight.Value = dval;
                inputWeight = null;
                inputEdge = null;
                edgeInputLabel.visible = false;
                return true;
            }

            return false;
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

            float zoomX = (renderFrame.width - 150.0f) / w;
            float zoomY = (renderFrame.height - 150.0f) / h;

            float zoom = zoomX < zoomY ? zoomX : zoomY;
            renderFrame.scale = (float)Math.Log(zoom, 4);
            renderFrame.CalcZoom();
        }
    }
}
