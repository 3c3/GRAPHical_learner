using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    /// <summary>
    /// Модификация на втория алгоритъм, по-бърза и по-малко сбива върхове
    /// </summary>
    public class ForceSimulatorMKIIB : ForceSimulatorMkII
    {
        public override event SimulatorStoppedHandler SimulatorStopped;

        static double optimalLength = 200;
        static int maxIterrations = 100;
        static double epsilon = 0.0001;

        double minEnergy = double.PositiveInfinity;
        int iterationsCount = 0;

        public ForceSimulatorMKIIB(double k, double c)
        {
            this.k = k;
            this.c = c;
            minStep = 0.5;
            step = minStep;
            stepScale = 0.8;
        }

        public override void SimulateStep()
        {
            base.SimulateStep();
            if(prevEnergy - minEnergy > epsilon)
            {
                minEnergy = prevEnergy;
                iterationsCount = 0;
            }
            else
            { // когато енергията не е намаляла с поне epsilon за iterationsCount
                iterationsCount++; // завъртания, графа се счита за наместен
                
                if(iterationsCount >= maxIterrations)
                {
                    if (SimulatorStopped != null) SimulatorStopped();
                    iterationsCount = 0;
                }
            }
        }

        protected override PVector GetAttractionForce(double dx, double dy)
        {
            double dist = CalculateDistance(dx, dy);

            double coef = dist >= optimalLength ? 1.0 : dist / optimalLength; // това е една от основните промени
            // силата на привличане намалява, когато разстоянието стане много малко. С това се постига по-малко сбиване
            double f = -1.0 * Math.Min((coef * dist * dist) / k, maxForce); // обратен знак -> привлича

            if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

            double fSin = dy / dist;
            double fCos = dx / dist;

            return new PVector(f * fCos, f * fSin);
        }

        protected override PVector GetRepulsionForce(double dx, double dy)
        {
            double dist = CalculateDistance(dx, dy);
            double ndist = dist < 0 ? dist * -1 : dist;
            double f = Math.Min(c * k * k * k * k / (ndist * ndist * ndist), maxForce);

            if (Double.IsNaN(f)) throw new ArithmeticException("Силата се прецака.");

            double fSin = dy / dist;
            double fCos = dx / dist;

            return new PVector(f * fCos, f * fSin);
        }

        int negProg = 0;

        protected override void UpdateStepLength(double energy)
        {
            if (energy < prevEnergy)
            {
                negProg = 0;
                progress++;
                if (progress >= 2)
                {
                    step /= stepScale;
                    if (step > maxStep) step = maxStep;
                    progress = 0;
                }
            }
            else
            {
                progress = 0;
                negProg++;
                if(negProg > 0)
                {
                    step *= stepScale;
                    if (step < minStep) step = minStep;
                    negProg = 0;
                }                
            }
            prevEnergy = energy;
        }

        public override void Reset()
        {
            minEnergy = double.PositiveInfinity;
        }
    }
}
