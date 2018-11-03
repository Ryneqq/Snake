using System;

namespace NeuralNetwork {
    public class Sin: IActivationFunction {
        public double Activate(double value) {
            return Math.Sin(value);
        }
    }
}