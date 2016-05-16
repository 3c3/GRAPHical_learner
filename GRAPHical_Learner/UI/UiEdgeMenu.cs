using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class UiEdgeMenu : UiVerticalMenu
    {
        private Edge edge;
        private Graph graph;
        private MainUI ui;

        public UiEdgeMenu(Edge e, Graph g, MainUI ui, float x, float y)
        {
            edge = e;
            graph = g;

            X = (int)x;
            Y = (int)y;

            this.ui = ui;

            AddItem("Премахни", RemoveEdge);
            AddItem("Промени тегло", EditEdge);
        }

        void RemoveEdge(UiComponent sender, Object args)
        {
            graph.RemoveEdge(edge);
            Remove();
        }

        void EditEdge(UiComponent sender, Object arg)
        {
            ui.edgeInputLabel.visible = true;
            ui.inputEdge = edge;

            if (Property.EdgeWeightId == -1) Property.SetSpecialProperty(Property.GetPropertyId("тегло"), SpecialProperty.EdgeWeight);

            if (edge.HasProperty(Property.EdgeWeightId))
            {
                object val = edge.GetPropertyValue(Property.EdgeWeightId);
                edge.SetProperty(Property.EdgeWeightId, val.ToString());
                ui.inputWeight = edge.GetProperty(Property.EdgeWeightId);
            }
            else
            {
                edge.SetProperty(Property.EdgeWeightId, "");
                ui.inputWeight = edge.GetProperty(Property.EdgeWeightId);
            }
        }
    }
}
