using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Ребро на граф
    /// </summary>
    public class Edge : PropertyHolder
    {
        private static int idCounter = 0;
        private static int arrowLenght = 15;
        private static float angle = 3.5f;

        public Vertex source, destination;
        public readonly int id;

        private Line line = new Line();

        private Line arrowLine1, arrowLine2;

        private ScalableLabel weightLabel = new ScalableLabel();

        private Property weightProperty;

        public object Weight
        {
            get { return weightProperty == null ? null : weightProperty.Value; }
        }

        public Edge(Vertex src, Vertex dest)
        {
            id = idCounter++;
            source = src;
            destination = dest;
        }

        public Edge(Vertex src, Vertex dest, bool directed)
        {
            id = idCounter++;
            source = src;
            destination = dest;
            if(directed)
            {
                arrowLine1 = new Line();
                arrowLine2 = new Line();
            }
        }

        /// <summary>
        /// Казва какво е
        /// </summary>
        /// <returns></returns>
        public override string GetName()
        {
            return String.Format("Ребро {0}-{1}({2})", source.id, destination.id, id);
        }

        public override void SetProperty(int propertyId, object value)
        {
            base.SetProperty(propertyId, value);
            if (weightProperty == null && propertyId == Property.edgeWeighId)
            {
                weightProperty = properties.Last();
            }
        }

        Object lastPropVal;
        /// <summary>
        /// Ако има промяна в теглото - обновява текста
        /// </summary>
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

        private void MakeArrow()
        {
            float sin, cos;
            line.GetTrigs(out sin, out cos);

            float destX = destination.x - 18 * cos;
            float destY = destination.y - 18 * sin;

            arrowLine1.SetSource(destX, destY);
            arrowLine1.SetDestination(destX + arrowLenght*cos, destY + arrowLenght*sin);

            arrowLine2.SetSource(destX, destY);
            arrowLine2.SetDestination(destX + arrowLenght * cos, destY + arrowLenght * sin);

            arrowLine1.Rotate(angle);
            arrowLine2.Rotate(-angle);
        }

        public bool HitCheck(Vector2f globalCoords)
        {
            float xLeft, yLeft, xRight, yRight;
            return false;
        }

        public void DrawSelf(RenderTarget window, RenderFrame rf)
        {
            CheckWeightProperty();

            line.SetDestination(destination.x, destination.y);
            line.SetSource(source.x, source.y);
            line.DrawSelf(window, rf);

            if(arrowLine1!=null)
            {
                MakeArrow();
                arrowLine1.DrawSelf(window, rf);
                arrowLine2.DrawSelf(window, rf);
            }

            if (weightProperty != null) weightLabel.DrawSelf(window, rf, (source.x + destination.x) / 2.0f, (source.y + destination.y) / 2.0f);
        }

        public static void ResetCounter()
        {
            idCounter = 0;
        }
    }
}
