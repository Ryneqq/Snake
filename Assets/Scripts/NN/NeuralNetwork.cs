using UnityEngine;
using System;

public class NeuralNetwork{
    private int input;
    private int[] layers;
    private Matrix[] nn;

    /// <summary>
    /// Constructor takes number of inputs and an array of numbers of neurons in each layer.
    /// </summary>
    public NeuralNetwork(int _input, int[] _layers){
        input = _input;
        layers = _layers;

        nn = new Matrix[layers.Length];
        nn[0] = Matrix.RandomMatrix(input, layers[0], 0.1f);    
        for(int i = 1; i < layers.Length; i++){
            nn[i+1] = Matrix.RandomMatrix(layers[i-1], layers[i], 0.1f);                 
        } 
        // Display your nn
        // for(int i = 0; i < nn.Length; i++){
        //     Debug.Log(neuralNetwork[i].ToString());             
        // }
    }

    /// <summary>
    /// Learning
    /// </summary>
    // IN:
    // OUT:
    public void Learn(Matrix P, Matrix T, int n){
        int examples = P.cols; // n.o. examples
        float lr = .1f; // learning rate

        for(int k = 0; k < n; k++){
            int next = UnityEngine.Random.Range(0, examples);

            string example = String.Empty;
            for(int i = 0; i < P.rows; i++)
                example += P[i,next] + "\r\n";
            Matrix signal = Matrix.Parse(example); 

            Matrix response = Run(signal);

            string anwser = String.Empty;
            for(int i = 0; i < T.rows; i++)
                anwser += T[i,next] + "\r\n";
            Matrix diff = Matrix.Parse(anwser) - response;
            Matrix error = ActivationFunction(response);
            for(int i = 0; i < diff.rows; i++){
                for(int j = 0; j < diff.cols; j++){
                    error[i,j] *= diff[i,j];
                }
            }
            Matrix dnn = signal * Matrix.Transpose(error);
            dnn = Matrix.Multiply(lr, dnn);
            nn[0] += dnn;
        }
    }

    public Matrix Run(Matrix X){
        Matrix U = Matrix.Transpose(nn[0]) * X;
        return ActivationFunction(U);
    }

    private Matrix ActivationFunction(Matrix U){
        // sigmoid:
        // 1/(1+exp(-beta*U))
        Matrix Y = U.Duplicate();
        int beta = 5;
        for(int i = 0; i < Y.rows; i++){
            for(int j = 0; j < Y.cols; j++){
                Y[i,j] = 1/(1+ System.Math.Exp(-beta * U[i,j]));
            }
        }
        return Y;
    }
    
    private Matrix ActivationFunction2(Matrix U){
        // ReLU
        Matrix Y = U.Duplicate();
        for(int i = 0; i < Y.rows; i++){
            for(int j = 0; j < Y.cols; j++){
                if(U[i,j] < 0)
                    Y[i,j] = 0;
                else 
                    Y[i,j] = U[i,j];
            }
        }
        return Y;
    }
}