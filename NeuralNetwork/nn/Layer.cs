using System;
using System.Linq;

namespace NeuralNetwork {
    // public class Layer: Matrix {
    //     public IActivationFunction afn;

    //     public Layer(Matrix layer, IActivationFunction afn)
    //     {
    //         this.afn = afn;

    //         base.rows = layer.rows;
    //         base.cols = layer.cols;
    //         base.mat  = layer.mat;
    //     }

    //     public Layer(int n, int m, IActivationFunction afn): this(Matrix.RandomMatrix(n, m, 100), afn) {}
    // }

    public class Layer {
        public IActivationFunction afn;

        public Layer() {
            // var rand = new Random();
            // var next = rand.Next(0, 4);

            this.afn = new Sigmoid();

            // switch (next)
            // {
            //     case 0:
            //         this.afn = new Sigmoid();
            //         break;
            //     case 1:
            //         this.afn = new ReLU();
            //         break;
            //     case 2:
            //         this.afn = new LinearCut();
            //         break;
            //     case 3:
            //         this.afn = new Sin();
            //         break;
            //     case 4:
            //         this.afn = new TanH();
            //         break;
            // }
        }

        public Layer(IActivationFunction afn)
        {
            this.afn = afn;
        }
    }
}