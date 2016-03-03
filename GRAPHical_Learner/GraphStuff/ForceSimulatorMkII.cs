using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class ForceSimulatorMkII : IForceSimulator
    {
        public virtual event SimulatorStoppedHandler SimulatorStopped;

        protected Graph graph;
        protected double k, c, step;

        private static double scale = 25.0;

        public ForceSimulatorMkII()
        {

        }

        public ForceSimulatorMkII(double k, double c)
        {
            this.k = k;
            this.c = c;
            step = minStep;
        }

        protected bool done = false;
        protected double prevEnergy;

        public virtual void SimulateStep()
        {
            //Console.WriteLine("simulating...");
            double energy = 0;
            foreach(Vertex v in graph.vertices)
            {
                PVector force = CalculateVertexForce(v);
                energy += force.CalcLengthSquared();
                v.x += (float)(force.x * step * scale);
                v.y += (float)(force.y * step * scale);
                //Console.WriteLine("{0};{1}", force.x * step, force.y * step);
            }

            UpdateStepLength(energy);
        }

        public void SetForce(float percent)
        {
            step = (maxStep - minStep) * percent / 100.0f + minStep;
        }

        protected int progress;
        protected double stepScale = 0.9f;
        protected double minStep = .1f;
        protected double maxStep = 20.0f;
        protected virtual void UpdateStepLength(double energy)
        {
            if(energy < prevEnergy)
            {
                progress++;
                if (progress >= 3)
                {
                    step /= stepScale;
                    if (step > maxStep) step = maxStep;
                    progress = 0;
                }
            }
            else
            {
                progress = 0;
                step *= stepScale;
                if (step < minStep) step = minStep;
            }
            prevEnergy = energy;
            //Console.WriteLine(String.Format("Energy: {0}; Step: {1}", energy, step));
        }

        protected double maxForce = 0.1;

        public PVector CalculateVertexForce(Vertex v)
        {
            PVector resultF = new PVector();
            foreach (Edge e in v.edges)
            { // смятаме пружинните сили(на ребрата) - привличат
                Vertex other = e.source == v ? e.destination : e.source;

                if (float.IsNaN(other.x) || float.IsNaN(other.y) || float.IsNaN(v.x) || float.IsNaN(v.y)) continue;

                double dx = v.x - other.x;
                double dy = v.y - other.y;

                resultF.Add(GetAttractionForce(dx, dy));
            }

            foreach (Vertex vo in graph.vertices)
            { // електрични сили
                if (v == vo) continue;

                if (float.IsNaN(vo.x) || float.IsNaN(vo.y) || float.IsNaN(v.x) || float.IsNaN(v.y)) continue;

                double dx = v.x - vo.x;
                double dy = v.y - vo.y;

                resultF.Add(GetRepulsionForce(dx, dy));
            }
            return resultF;
        }

        protected virtual PVector GetAttractionForce(double dx, double dy)
        {
            double dist = CalculateDistance(dx, dy);
            double f = -1.0 * Math.Min((dist * dist) / k, maxForce); // обратен знак -> привлича

            if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

            double fSin = dy / dist;
            double fCos = dx / dist;

            return new PVector(f * fCos, f * fSin);
        }

        protected virtual PVector GetRepulsionForce(double dx, double dy)
        {
            double dist = CalculateDistance(dx, dy);
            double ndist = dist < 0 ? dist * -1 : dist;
            double f = Math.Min(c * k * k * k * k / (ndist * ndist * ndist), maxForce);

            if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

            double fSin = dy / dist;
            double fCos = dx / dist;

            return new PVector(f * fCos, f * fSin);
        }

        public double CalculateDistance(double dx, double dy)
        {
            dx /= scale;
            dy /= scale;
            return (double)Math.Sqrt(dx * dx + dy * dy);
        }

        public void SetGraph(Graph graph)
        {
            this.graph = graph;
        }

        public virtual void Reset()
        {

        }
    }
}
