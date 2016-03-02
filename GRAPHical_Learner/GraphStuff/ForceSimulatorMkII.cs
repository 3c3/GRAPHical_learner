using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class ForceSimulatorMkII : IForceSimulator
    {
        private Graph graph;
        private double k, c, step;

        private static double scale = 25.0;

        public ForceSimulatorMkII(double k, double c)
        {
            this.k = k;
            this.c = c;
            step = minStep;
        }

        bool done = false;
        double prevEnergy;

        public void SimulateStep()
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
            step = (maxStep - minStep) * 100.0f / percent + minStep;
        }

        int progress;
        double stepScale = 0.9f;
        double minStep = .1f;
        double maxStep = 20.0f;
        void UpdateStepLength(double energy)
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

        double maxForce = 0.1;

        public PVector CalculateVertexForce(Vertex v)
        {
            PVector resultF = new PVector();
            foreach (Edge e in v.edges)
            { // смятаме пружинните сили(на ребрата) - привличат
                Vertex other = e.source == v ? e.destination : e.source;

                if (float.IsNaN(other.x) || float.IsNaN(other.y) || float.IsNaN(v.x) || float.IsNaN(v.y)) continue;

                double dx = v.x - other.x;
                double dy = v.y - other.y;

                double dist = CalculateDistance(dx, dy);
                double ndist = dist < 0 ? dist * -1 : dist;
                double f = -1.0 * Math.Min((ndist*ndist) / k, maxForce); // обратен знак -> привлича

                if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

                

                double fSin = dy / dist;
                double fCos = dx / dist;

                resultF.Add(new PVector(f * fCos, f * fSin));
            }

            foreach (Vertex vo in graph.vertices)
            { // електрични сили
                if (v == vo) continue;

                if (float.IsNaN(vo.x) || float.IsNaN(vo.y) || float.IsNaN(v.x) || float.IsNaN(v.y)) continue;

                double dx = v.x - vo.x;
                double dy = v.y - vo.y;

                double dist = CalculateDistance(dx, dy);
                double ndist = dist < 0 ? dist * -1 : dist;
                double f = Math.Min(c * k * k * k * k / (ndist*ndist*ndist), maxForce);

                if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

                

                double fSin = dy / dist;
                double fCos = dx / dist;

                resultF.Add(new PVector(f * fCos, f * fSin));
            }
            return resultF;
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


    }
}
