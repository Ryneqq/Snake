using System;

namespace NeuralNetwork {
    public class LinearCut: IActivationFunction {
        public double Activate(double value) {
            if(value < -1)
                return -1;
            else if (value > 1)
                return 1;
            else 
                return value;
        }
    }
}