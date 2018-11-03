using System;

namespace NeuralNetwork {
    public class ReLU: IActivationFunction {
        private double beta;

        public ReLU() {
            this.beta = (double)new Random().Next(1, 300) / 100;
        }
        public double Activate(double value) {
            var outcome = beta * value;

            if(outcome < 0)
                return 0;
            else if(outcome > 1)
                return 1;
            else 
                return outcome;
        }
    }
}