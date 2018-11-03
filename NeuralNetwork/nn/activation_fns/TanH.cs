using System;

namespace NeuralNetwork {
    public class TanH: IActivationFunction {
        private double beta;

        public TanH() {
            this.beta = (double)new Random().Next(1, 300) / 100;
        }

        public double Activate(double value) {
            return 2/(1+ System.Math.Exp(-beta * value)) - 1;
        }
    }
}