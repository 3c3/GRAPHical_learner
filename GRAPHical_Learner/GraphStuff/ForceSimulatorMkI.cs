using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Първи опит за алгоритъм за подреждане.
    /// Прекалено нестабилен.
    /// </summary>
    public class ForceSimulatorMkI : IForceSimulator
    {
        public event SimulatorStoppedHandler SimulatorStopped;
        public void Reset()
        {

        }

        private float electricalConst, springConst, springLength, friction, airRes;
        private float k = 1.0f, c = .2f;
        private Graph graph;

        private static float minSpeed = 0.00001f;
        private static float maxSpeed = .8f;

        public ForceSimulatorMkI(float el, float spring, float springL, float friction, float airRes)
        {
            electricalConst = el;
            springConst = spring;
            springLength = springL;
            this.friction = friction;
            this.airRes = airRes;
        }

        public void SetForce(float percent)
        {

        }

        public void SimulateStep()
        {
            graph.vertices.ForEach(v => SimulateVertex(v)); // обновява скоростите

            foreach(Vertex v in graph.vertices)
            {
                v.velocity.Multiply(friction); // прилага триене за да се свърши

                float vel = (float)Math.Sqrt(v.velocity.CalcLengthSquared());
                float airFactor = Math.Min(1.0f - vel / maxSpeed, 0.5f);

               /* v.velocity.x *= airFactor;
                v.velocity.y *= airFactor;*/

                if (v.velocity.CalcLengthSquared() < minSpeed*minSpeed) continue; // обновява позициите, ако скоростта е достатъчна

                v.x += (float)v.velocity.x;
                v.y += (float)v.velocity.y;
            }
        }

        
        public void SimulateVertex(Vertex v)
        {
            foreach(Edge e in v.edges)
            { // смятаме пружинните сили(на ребрата) - привличат
                Vertex other = e.source == v ? e.destination : e.source;

                float dist = CalculateDistance(v, other);
                float ndist = dist < 0 ? dist*-1 : dist;
                float f = -CalculateSpringForce(ndist); // обратен знак -> привлича

                float dx = v.x - other.x;
                float dy = v.y - other.y;

                float fSin = dy / dist;
                float fCos = dx / dist;

                v.velocity.Add(new PVector(f * fCos, f * fSin));
            }

            foreach(Vertex vo in graph.vertices)
            { // електрични сили
                if (v == vo) continue;

                float dist = CalculateDistance(v, vo);
                float ndist = dist < 0 ? dist * -1 : dist;
                float f = CalculateElectricForce(ndist);

                float dx = v.x - vo.x;
                float dy = v.y - vo.y;

                float fSin = dy / dist;
                float fCos = dx / dist;

                v.velocity.Add(new PVector(f * fCos, f * fSin));
            }
        }

        public float CalculateDistance(Vertex a, Vertex b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float CalculateElectricForce(float dist)
        {
            return electricalConst / (dist * dist);
        }

        public float CalculateSpringForce(float dist)
        {
            return (dist - springLength) * springConst;
        }

        public void SetGraph(Graph graph)
        {
            this.graph = graph;
        }
    }
}
