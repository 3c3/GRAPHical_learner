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

        private Line line = new Line();
        private ScalableLabel weightLabel = new ScalableLabel();

        private Property weightProperty;

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

        public override void SetProperty(int propertyId, object value)
        {
            base.SetProperty(propertyId, value);
            if (weightProperty == null && propertyId == Property.vertexColorId)
            {
                weightProperty = properties.Last();
            }
        }

        Object lastPropVal;
        void CheckWeightProperty()
        {
            if (weightProperty == null) return;

            if(lastPropVal == null)
            {
                lastPropVal = weightProperty.Value;
                weightLabel.Text = lastPropVal.ToString();
            }
            else if(lastPropVal != weightProperty.Value)
            {
                lastPropVal = weightProperty.Value;
                weightLabel.Text = lastPropVal.ToString();
            }
        }

        public void DrawSelf(RenderWindow window, RenderFrame rf)
        {
            CheckWeightProperty();

            line.SetDestination(destination.x, destination.y);
            line.SetSource(source.x, source.y);
            line.DrawSelf(window, rf);

            if (weightProperty != null) weightLabel.DrawSelf(window, rf, (source.x + destination.x) / 2.0f, (source.y + destination.y) / 2.0f);
        }

        public static void ResetCounter()
        {
            idCounter = 0;
        }
    }
}
