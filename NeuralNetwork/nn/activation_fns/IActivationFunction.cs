using System;

namespace NeuralNetwork {
    public interface IActivationFunction {
        double Activate(double value);
    }
}