using System;

namespace NeuralNetwork {
    public class Sigmoid: IActivationFunction {
        private double beta;

        public Sigmoid() {
            this.beta = (double)new Random().Next(1, 300) / 100;
        }
        public double Activate(double value) {
            return 1/(1+ System.Math.Exp(-this.beta * value));
        }
    }
}